namespace Fanex.ProductStatus.Setting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CommandServices;
    using DataAccess;
    using Models;
    using Shared;

    public interface ProductStatusService
    {
        IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId);

        Result UpdateProductStatus(
            int productId,
            int custId,
            byte custRole,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands);

        IEnumerable<Result> MultipleUpdateProductStatus(
            int productId,
            byte custRole,
            int[] custIds,
            string[] custNames,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands);
    }

    internal class ProductStatusServiceImpl : ProductStatusService
    {
        private readonly ProductStatusDataAccess statusDataAccess;
        private readonly CriteriaFactoryService criteriaFactory;
        private readonly RefDataServiceProvider refDataServiceProvider;

        public ProductStatusServiceImpl(
            ProductStatusDataAccess statusDataAccess = null,
            CriteriaFactoryService criteriaFactory = null,
            RefDataServiceProvider refDataServiceProvider = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
            this.criteriaFactory = criteriaFactory ?? new CriteriaFactoryServiceImpl();
            this.refDataServiceProvider = refDataServiceProvider ?? new RefDataServiceProviderImpl();
        }

        public IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId)
        {
            var criteria = criteriaFactory.CreateSelectCriteria(productId, custId);
            var rawData = statusDataAccess.GetProductStatus(criteria).ToList();

            return SetProductStatusName(productId, rawData);
        }

        private IEnumerable<ProductStatusModel> SetProductStatusName(
            int productId,
            IList<ProductStatusModel> rawData)
        {
            var setNameByBetTypeId = rawData.Count > 1;

            foreach (var item in rawData)
            {
                item.Name = setNameByBetTypeId
                    ? refDataServiceProvider.GetBetTypeName(item.BetType)
                    : refDataServiceProvider.GetProductName(productId);

                yield return item;
            }
        }

        public Result UpdateProductStatus(
            int productId,
            int custId,
            byte custRole,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            var commandService = ProductStatusCommandServiceFactory.ContainsKey(productId)
                ? ProductStatusCommandServiceFactory[productId]
                : new DefaultProductStatusCommandService();

            return commandService.UpdateProductStatus(
                productId,
                custId,
                custRole,
                actorId,
                actorName,
                ip,
                commands);
        }

        private static IDictionary<int, ProductStatusCommandService> ProductStatusCommandServiceFactory
            = new Dictionary<int, ProductStatusCommandService>
            {
                [ProductIdentity.Casino] = new CasinoProductStatusCommandService(),
                [ProductIdentity.Keno] = new KenoProductStatusCommandService()
            };

        public IEnumerable<Result> MultipleUpdateProductStatus(
            int productId,
            byte custRole,
            int[] custIds,
            string[] custNames,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            Result result;
            int customerId;
            int custIdsLength = custIds.Length;

            for (int i = 0; i < custIdsLength; i++)
            {
                customerId = custIds[i];
                result = UpdateProductStatus(productId, customerId, custRole, actorId, actorName, ip, commands);

                yield return new MultipleResult(result, customerId, custNames[i]);
            }
        }
    }
}