namespace Fanex.ProductStatus.Setting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess;
    using Models;
    using Shared;

    public interface ProductStatusService
    {
        IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId, byte custRoleId);

        Result UpdateProductStatus(
            int productId,
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands);

        IEnumerable<Result> MultipleUpdateProductStatus(
            int productId,
            IEnumerable<Customer> customers,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands);
    }

    internal class ProductStatusServiceImpl : ProductStatusService
    {
        private readonly ProductStatusDataAccess statusDataAccess;
        private readonly CriteriaFactoryService criteriaFactory;
        private readonly RefDataService refDataService;

        public ProductStatusServiceImpl(
            ProductStatusDataAccess statusDataAccess = null,
            CriteriaFactoryService criteriaFactory = null,
            RefDataService refDataService = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
            this.criteriaFactory = criteriaFactory ?? new CriteriaFactoryServiceImpl();
            this.refDataService = refDataService ?? new RefDataServiceImpl();
        }

        public IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId, byte custRoleId)
        {
            var criteria = criteriaFactory.CreateSelectCriteria(productId, custId);
            var rawData = statusDataAccess
                .GetProductStatus(criteria, custRoleId)
                .ToList();

            var rawDataCount = rawData.Count;

            if (rawDataCount == 1)
            {
                return GetProductStatusProductName(productId, rawData);
            }

            return GetProductStatusBetTypeName(productId, rawData);
        }

        public Result UpdateProductStatus(
            int productId,
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            return productId == ProductIdentity.Keno ?
                UpdateKenoStatus(custId, actorId, ip, commands) :
                UpdateStatus(productId, custId, actorId, ip, commands);
        }

        public IEnumerable<Result> MultipleUpdateProductStatus(
            int productId,
            IEnumerable<Customer> customers,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            IList<Result> results = new List<Result>();

            foreach (var customer in customers)
            {
                // Todo: check product is offered to customer here.
                Result result = new MultipleResult(1, "Error", customer.CustId, customer.CustName);

                if (result.IsSuccess())
                {
                    result = UpdateProductStatus(productId, customer.CustId, actorId, ip, commands);
                }

                results.Add(result);
            }

            return results;
        }

        private IEnumerable<ProductStatusModel> GetProductStatusProductName(
            int productId,
            IEnumerable<ProductStatusModel> entities)
        {
            foreach (var item in entities)
            {
                item.ProductId = productId;
                item.Name = refDataService.GetProductName(productId);

                yield return item;
            }
        }

        private IEnumerable<ProductStatusModel> GetProductStatusBetTypeName(
            int productId,
            IEnumerable<ProductStatusModel> entities)
        {
            foreach (var item in entities)
            {
                item.ProductId = productId;
                item.Name = refDataService.GetBetTypeName(item.BetType);

                yield return item;
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
    }
}