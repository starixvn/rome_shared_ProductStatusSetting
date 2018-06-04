namespace Fanex.ProductStatus.Setting.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly RnGCasinoService rnGCasinoService;

        private const int SystemId = 2;

        public ProductStatusServiceImpl(
            ProductStatusDataAccess statusDataAccess = null,
            CriteriaFactoryService criteriaFactory = null,
            RefDataServiceProvider refDataServiceProvider = null,
            RnGCasinoService rnGCasinoService = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
            this.criteriaFactory = criteriaFactory ?? new CriteriaFactoryServiceImpl();
            this.refDataServiceProvider = refDataServiceProvider ?? new RefDataServiceProviderImpl();
            this.rnGCasinoService = rnGCasinoService ?? new RnGCasinoServiceImpl(this.statusDataAccess);
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
            switch(productId)
            {
                case ProductIdentity.Keno:
                    return UpdateKenoStatus(custId, actorId, ip, commands);
                case ProductIdentity.Casino:
                    return UpdateCasinoStatus(productId, custId, custRole, actorId, actorName, ip, commands);
                default:
                    return UpdateStatus(productId, custId, actorId, ip, commands);
            }
        }

        private Result UpdateKenoStatus(
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            IList<Result> results = new List<Result>();
            var criteria = criteriaFactory.CreateUpdateKenoStatusCriteria(custId, actorId, ip, commands);

            foreach (var item in criteria)
            {
                results.Add(statusDataAccess.ExecuteNonQuery(item));
            }

            var errorResult = results.FirstOrDefault(r => !r.IsSuccess());

            return errorResult ?? results[0];
        }

        private Result UpdateStatus(
            int productId,
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            var criteria = criteriaFactory.CreateUpdateCriteria(productId, custId, actorId, ip, commands);
            return statusDataAccess.ExecuteNonQuery(criteria);
        }

        private Result UpdateCasinoStatus(
         int productId,
         int custId,
         byte custRole,
         int actorId,
         string actorName,
         string ip,
         IEnumerable<ProductStatusCommand> commands)
        {
            var criteria = criteriaFactory.CreateUpdateCriteria(productId, custId, actorId, ip, commands);
            var result = statusDataAccess.ExecuteNonQuery(criteria);

            if (result.IsSuccess())
            {
                UpdateCasinoStatus(custId, custRole, actorId, actorName, ip, commands.First());
            }

            return result;
        }

        private  void UpdateCasinoStatus(int custId, byte custRole, int actorId, string actorName, string ip, ProductStatusCommand command)
            => rnGCasinoService.UpdateEnableCasinoByLevelUpdateEnableCasinoByLevel(
                custId,
                custRole,
                !command.Disable,
                command.UpdateAllMasters,
                command.UpdateAllAgents,
                command.UpdateAllMembers,
                !command.DisableAllMasters,
                !command.DisableAllAgents,
                !command.DisableAllMembers,
                ip,
                actorId,
                actorName,
                SystemId);

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
                result = UpdateProductStatus(productId, customerId, custRole, actorId, actorName, ip,commands);

                yield return new MultipleResult(result, customerId, custNames[i]);
            }
        }
    }
}