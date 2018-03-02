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
            var rawData = statusDataAccess.GetProductStatus(criteria);

            return SetProductStatusName(productId, rawData);
        }

        private IEnumerable<ProductStatusModel> SetProductStatusName(
            int productId,
            IEnumerable<ProductStatusModel> rawData)
        {
            var setNameByBetTypeId = rawData.Count() > 1;

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