using System.Collections.Generic;
using System.Linq;
using Fanex.ProductStatus.Setting.DataAccess;
using Fanex.ProductStatus.Setting.Enumerations;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    public class ProductStatusService : IProductStatusService
    {
        private readonly IProductStatusDataAccess statusDataAccess;
        private readonly IProductStatusCriteriaFactory criteriaFactory;
        private readonly IRefDataService refDataService;

        public ProductStatusService(
            IProductStatusDataAccess statusDataAccess = null,
            IProductStatusCriteriaFactory criteriaFactory = null,
            IRefDataService refDataService = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccess();
            this.criteriaFactory = criteriaFactory ?? new ProductStatusCriteriaFactory();
            this.refDataService = refDataService ?? new RefDataService();
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

        public Result UpdateProductStatus(int productId, IEnumerable<UpdateProductStatusParam> commands)
        {
            return productId == (int)ProductIdentity.Keno ?
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

        private Result UpdateKenoStatus(IEnumerable<UpdateProductStatusParam> commands)
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

        private Result UpdateStatus(int productId, IEnumerable<UpdateProductStatusParam> commands)
        {
            var criteria = criteriaFactory.CreateUpdateCriteria(productId, commands);

            return statusDataAccess.ExecuteNonQuery(criteria);
        }
    }
}
