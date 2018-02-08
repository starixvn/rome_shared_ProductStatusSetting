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

        Result UpdateProductStatus(int productId, IEnumerable<ProductStatusCommand> commands);
    }

    public class ProductStatusServiceImpl : ProductStatusService
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

        public IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId)
        {
            var criteria = criteriaFactory.CreateSelectCriteria(productId, custId);
            var rawData = statusDataAccess
                .GetProductStatus(criteria)
                .ToList();

            var rawDataCount = rawData.Count;

            if (rawDataCount == 1)
            {
                return GetProductStatusProductName(productId, rawData);
            }

            return GetProductStatusBetTypeName(productId, rawData);
        }

        public Result UpdateProductStatus(int productId, IEnumerable<ProductStatusCommand> commands)
        {
            return productId == ProductIdentity.Keno ?
                UpdateKenoStatus(commands) :
                UpdateStatus(productId, commands);
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

        private Result UpdateKenoStatus(IEnumerable<ProductStatusCommand> commands)
        {
            IList<Result> results = new List<Result>();
            var criteria = criteriaFactory.CreateUpdateKenoStatusCriteria(commands);

            foreach (var item in criteria)
            {
                results.Add(statusDataAccess.ExecuteNonQuery(item));
            }

            var errorResult = results.FirstOrDefault(r => !r.IsSuccess());

            return errorResult ?? results[0];
        }

        private Result UpdateStatus(int productId, IEnumerable<ProductStatusCommand> commands)
        {
            var criteria = criteriaFactory.CreateUpdateCriteria(productId, commands);

            return statusDataAccess.ExecuteNonQuery(criteria);
        }
    }
}