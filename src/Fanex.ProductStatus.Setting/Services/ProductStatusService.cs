using System.Collections.Generic;
using System.Linq;
using Fanex.ProductStatus.Setting.AppResources;
using Fanex.ProductStatus.Setting.DataAccess;
using Fanex.ProductStatus.Setting.Enumerations;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    internal class ProductStatusService : IProductStatusService
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

            var rawData = statusDataAccess.GetProductStatus(criteria);
            var rawDataCount = rawData.Count();

            if (rawDataCount == 1)
            {
                return GetProductStatusProductName(productId, rawData);
            }

            return GetProductStatusBetTypeName(productId, rawData);
        }

        public Result UpdateProductStatus(UpdateProductStatusCommand updateCommand)
        {
            return updateCommand.ProductId == (int)ProductIdentity.Keno ?
                UpdateKenoStatus(updateCommand) :
                UpdateStatus(updateCommand);
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
                item.BetTypeName = GetBetTypeName(item.BetType);

                yield return item;
            }
        }

        private string GetBetTypeName(int betTypeId)
        {
            var betType = (BetTypeIdentity)betTypeId;

            if (betType == BetTypeIdentity.AllbetStartRange)
            {
                return Resources.BetType_2401;
            }

            return refDataService.GetBetTypeName(betTypeId);
        }

        private Result UpdateKenoStatus(UpdateProductStatusCommand updateCommand)
        {
            Result result;
            IList<Result> errors = new List<Result>();
            var criteria = criteriaFactory.CreateUpdateKenoStatusCriteria(updateCommand);

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

        private Result UpdateStatus(UpdateProductStatusCommand updateCommand)
        {
            var criteria = criteriaFactory.CreateUpdateCriteria(updateCommand);
            var result = statusDataAccess.ExecuteNonQuery(criteria);

            if (result.IsSuccess())
            {
                result.Message = Resources.StatusUpdatedSuccessfully;
            }

            return result;
        }
    }
}
