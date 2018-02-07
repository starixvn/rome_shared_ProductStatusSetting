using System.Collections.Generic;
using System.Linq;
using Fanex.ProductStatus.Setting.AppResources;
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

        public virtual Result UpdateProductStatus(IEnumerable<ProductStatusCommand> commands)
        {
            var firstCommand = commands.First();

            return firstCommand.ProductId == (int)ProductIdentity.Keno ?
                UpdateKenoStatus(commands) :
                UpdateStatus(commands);
        }

        private static Result ParseKenoErrorResults(IEnumerable<Result> errors)
        {
            IList<Result> flattenErrors = new List<Result>();
            var groupErrorCode = errors.ToLookup(err => err.Code);

            foreach (var group in groupErrorCode)
            {
                flattenErrors.Add(Result.CreateErrorResult(group.Key, group.First().Message));
            }

            return Result.CreateErrorResult(
                flattenErrors[0].Code,
                string.Join("<br/>", flattenErrors.Select(err => err.Message)));
        }

        private IEnumerable<ProductStatusModel> GetProductStatusProductName(
            int productId,
            IEnumerable<ProductStatusModel> entities)
        {
            foreach (var item in entities)
            {
                item.ProductId = productId;
                item.BetTypeName = refDataService.GetProductName(productId);

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
                item.BetTypeName = refDataService.GetBetTypeName(item.BetType);

                yield return item;
            }
        }

        private Result UpdateKenoStatus(IEnumerable<ProductStatusCommand> commands)
        {
            Result result;
            IList<Result> errors = new List<Result>();
            var criteria = criteriaFactory.CreateUpdateKenoStatusCriteria(commands);

            foreach (var item in criteria)
            {
                result = statusDataAccess.ExecuteNonQuery(item);

                if (!result.IsSuccess())
                {
                    errors.Add(result);
                }
            }

            return errors.Count == 0 ?
                Result.CreateSuccessResult(Resources.StatusUpdatedSuccessfully) :
                ParseKenoErrorResults(errors);
        }

        private Result UpdateStatus(IEnumerable<ProductStatusCommand> commands)
        {
            var criteria = criteriaFactory.CreateUpdateCriteria(commands);
            var result = statusDataAccess.ExecuteNonQuery(criteria);

            if (result.IsSuccess())
            {
                result.Message = Resources.StatusUpdatedSuccessfully;
            }

            return result;
        }
    }
}
