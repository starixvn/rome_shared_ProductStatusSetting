namespace Fanex.ProductStatus.Setting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandServices;
    using Criteria;
    using DataAccess;
    using Fanex.ProductStatus.Setting.AppResources;
    using Models;
    using Shared;

    public interface ProductStatusService
    {
        IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId, int sportType, int systemId);

        Result UpdateProductStatus(
            int productId,
            int custId,
            byte custRole,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands,
            byte systemId,
            bool isMultiple = false);

        IEnumerable<Result> MultipleUpdateProductStatus(
            int productId,
            byte custRole,
            int[] custIds,
            string[] custNames,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands,
            byte systemId);
    }

    internal class ProductStatusServiceImpl : ProductStatusService
    {
        private readonly ProductStatusDataAccess statusDataAccess;
        private readonly CriteriaFactoryService criteriaFactory;
        private readonly RefDataService refDataService;
        private readonly IProductProfileService productProfileService;

        public ProductStatusServiceImpl(
            ProductStatusDataAccess statusDataAccess = null,
            CriteriaFactoryService criteriaFactory = null,
            RefDataService refDataService = null,
            IProductProfileService productProfileService = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
            this.criteriaFactory = criteriaFactory ?? new CriteriaFactoryServiceImpl();
            this.refDataService = refDataService ?? new RefDataServiceImpl();
            this.productProfileService = productProfileService ?? new ProductProfileService();
        }

        public IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId, int sportType, int systemId)
        {
            var criteria = criteriaFactory.CreateSelectCriteria(productId, custId, sportType);
            var rawData = statusDataAccess.GetProductStatus(criteria).ToList();

            return SetProductStatusName(productId, systemId, rawData);
        }

        private IEnumerable<ProductStatusModel> SetProductStatusName(
            int productId,
            int systemId,
            IList<ProductStatusModel> rawData)
        {
            var settingLevel = productProfileService.GetSettingLevel(productId, systemId);

            foreach (var item in rawData)
            {
                item.Name = SetProductStatusName((byte)settingLevel, productId, item.SportType, item.BetType);
                yield return item;
            }
        }

        private string SetProductStatusName(byte settingLevel, int productId, int sportType, int betType)
        {
            if (settingLevel == ProductSettingLevel.Product)
            {
                return refDataService.GetProductName(productId);
            }

            if (settingLevel == ProductSettingLevel.SportType)
            {
                return refDataService.GetSportTypeName(sportType);
            }

            if (settingLevel == ProductSettingLevel.BetType)
            {
                return refDataService.GetBetTypeName(betType);
            }

            return refDataService.GetBetTypeName(betType);
        }

        public Result UpdateProductStatus(
            int productId,
            int custId,
            byte custRole,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands,
            byte systemId,
            bool isMultiple = false)
        {
            var result = ProductStatusCommandServiceFactory(productId).UpdateProductStatus(
                productId,
                custId,
                custRole,
                actorId,
                actorName,
                ip,
                commands);

            if (result.IsSuccess() && IsDCSProduct(productId) && !isMultiple)
            {
                var umMessage = GetUMMessage(productId, custId, commands, systemId);

                if (!string.IsNullOrWhiteSpace(umMessage))
                {
                    result = new Result(Result.UMErrorCode, umMessage);
                }
            }

            return result;
        }

        private ProductStatusCommandService ProductStatusCommandServiceFactory(int productId)
        {
            switch (productId)
            {
                case ProductIdentity.Casino:
                    return new CasinoProductStatusCommandService(statusDataAccess);

                case ProductIdentity.Keno:
                    return new KenoProductStatusCommandService(statusDataAccess, refDataService);

                default:
                    return new DefaultProductStatusCommandService(statusDataAccess, criteriaFactory);
            }
        }

        public IEnumerable<Result> MultipleUpdateProductStatus(
            int productId,
            byte custRole,
            int[] custIds,
            string[] custNames,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands,
            byte systemId)
        {
            var umResults = new List<MultipleResult>();
            var otherResults = new List<MultipleResult>();
            var custNameHash = new Dictionary<int, string>();

            for (int i = 0, custIdsLength = custIds.Length; i < custIdsLength; i++)
            {
                var customerId = custIds[i];
                var customerName = custNames[i];
                custNameHash[customerId] = customerName;

                var result = UpdateProductStatus(productId, customerId, custRole, actorId, actorName, ip, commands, systemId, true);

                if (result.IsUM())
                {
                    umResults.Add(new MultipleResult(result, customerId, customerName));
                }
                else
                {
                    otherResults.Add(new MultipleResult(result, customerId, customerName));
                }
            }

            if (IsDCSProduct(productId))
            {
                umResults.AddRange(GetUMMessage(productId, custIds, custNameHash, commands, systemId));
            }
            else if (umResults.Count > 0)
            {
                umResults = GetUMMessage(productId, umResults.Select(r => r.CustId).ToArray(), custNameHash, commands, systemId).ToList();
            }

            otherResults.AddRange(umResults);

            return otherResults;
        }

        private string GetUMMessage(int productId, int custId, IEnumerable<ProductStatusCommand> commands, byte systemId)
        {
            var productStatusUM = statusDataAccess
                .GetProductStatusUM(new CheckProducStatusUMCriteria(productId, custId))
                .ToList();

            if (productStatusUM.Count == 0)
            {
                return string.Empty;
            }

            IList<string> umMessages = new List<string>();
            var settingLevel = (byte)productProfileService.GetSettingLevel(productId, systemId);

            foreach (var item in commands)
            {
                var message = productStatusUM.FirstOrDefault(m => m.SportType == 0 || m.SportType == item.SportType);

                if (message != null)
                {
                    umMessages.Add(BuildUMMessage(message, settingLevel, productId, item.SportType, item.BetType));
                }
            }

            return string.Join("<br/>", umMessages);
        }

        private string BuildUMMessage(ProductStatusUM message, byte settingLevel, int productId, int sportType, int betType)
            => string.Format(
                Resources.UMProductStatusPattern,
                SetProductStatusName(settingLevel, productId, sportType, betType),
                message.BeginTime.ToLongDateWithSlash(),
                message.EndTime.ToLongDateWithSlash());

        private IEnumerable<MultipleResult> GetUMMessage(
            int productId,
            int[] custIds,
            IDictionary<int, string> custNameHash,
            IEnumerable<ProductStatusCommand> commands,
            byte systemId)
        {
            var productStatusUM = statusDataAccess
                .GetProductStatusUM(new CheckProducStatusUMCriteria(productId, string.Join(",", custIds)))
                .GroupBy(m => new { m.BeginTime, m.EndTime, m.SportType, m.BetType })
                .ToList();

            if (productStatusUM.Count == 0)
            {
                yield break;
            }

            var settingLevel = (byte)productProfileService.GetSettingLevel(productId, systemId);

            foreach (var grp in productStatusUM)
            {
                var umMessages = new List<string> { Resources.UMProductStatusMultiple };

                if (productId == ProductIdentity.Keno)
                {
                    umMessages.Add(BuildUMMessage(ProductSettingLevel.Product, productId, 0,0, grp.Key.BeginTime, grp.Key.EndTime));
                }
                else
                {
                    umMessages.AddRange(
                        commands.Select(cmd => BuildUMMessage(settingLevel, productId, cmd.SportType, cmd.BetType, grp.Key.BeginTime, grp.Key.EndTime)));
                }

                yield return new MultipleResult(
                    new Result(Result.UMErrorCode, string.Join("<br/>", umMessages)),
                    0,
                    GetCustomerNameFromCustIds(grp.Select(m => m.CustId), custNameHash));
            }
        }

        private string BuildUMMessage(
            byte settingLevel,
            int productId,
            int sportType,
            int betType,
            DateTime beginTime,
            DateTime endTime)
            => string.Format(
                Resources.UMProductStatusPatternMultiple,
                SetProductStatusName(settingLevel, productId, sportType, betType),
                beginTime.ToLongDateWithSlash(),
                endTime.ToLongDateWithSlash());

        private static string GetCustomerNameFromCustIds(IEnumerable<int> custIds, IDictionary<int, string> custNameHash)
        {
            string custName;
            IList<string> customerNames = new List<string>();

            foreach (var item in custIds)
            {
                if (!custNameHash.TryGetValue(item, out custName))
                {
                    custName = item.ToString();
                }

                customerNames.Add(custName);
            }

            return string.Join(", ", customerNames);
        }

        private static bool IsDCSProduct(int productId)
            => !IsNonDCSProduct(productId);

        private static bool IsNonDCSProduct(int productId)
            => productId == ProductIdentity.Casino
            || productId == ProductIdentity.Keno
            || productId == ProductIdentity.NumberGame
            || productId == ProductIdentity.GoldDeluxe;
    }
}