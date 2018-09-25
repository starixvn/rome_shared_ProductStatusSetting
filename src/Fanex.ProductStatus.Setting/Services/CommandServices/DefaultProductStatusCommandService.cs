namespace Fanex.ProductStatus.Setting.Services.CommandServices
{
    using System.Collections.Generic;
    using DataAccess;
    using Models;

    internal class DefaultProductStatusCommandService : ProductStatusCommandService
    {
        private readonly ProductStatusDataAccess statusDataAccess;
        private readonly CriteriaFactoryService criteriaFactory;
        private readonly RefDataService refDataService;

        public DefaultProductStatusCommandService(
            ProductStatusDataAccess statusDataAccess = null,
            CriteriaFactoryService criteriaFactory = null,
            RefDataService refDataService = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
            this.criteriaFactory = criteriaFactory ?? new CriteriaFactoryServiceImpl();
            this.refDataService = refDataService ?? new RefDataServiceImpl();
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
            var criteria = criteriaFactory.CreateUpdateCriteria(productId, custId, actorId, ip, commands);
            var result = statusDataAccess.ExecuteNonQuery(criteria);
            result.ParseDCSProductMessage(productId, refDataService.GetBetTypeName);

            return result;
        }
    }
}
