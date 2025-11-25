namespace Fanex.ProductStatus.Setting.Services.CommandServices
{
    using System.Collections.Generic;
    using System.Text;
    using AppResources;
    // TODO: Requires Fanex.Common migration
    // using Common;
    using Criteria;
    using DataAccess;
    using Models;

    internal class KenoProductStatusCommandService : ProductStatusCommandService
    {
        private readonly ProductStatusDataAccess statusDataAccess;
        private readonly RefDataService refDataService;

        public KenoProductStatusCommandService(
            ProductStatusDataAccess statusDataAccess = null,
            RefDataService refDataService = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
            this.refDataService = refDataService ?? new RefDataServiceImpl();
        }

        public Result UpdateProductStatus(
            int productId,
            int custId,
            short custRole,
            int actorId,
            string actorName,
            string ip,
            byte settingStatus,
            IEnumerable<ProductStatusCommand> commands)
        {
            var criteria = CreateUpdateCriteria(custId, actorId, ip, commands);
            var errorMessages = new StringBuilder();
            var errorCode = 0;

            foreach (var item in criteria)
            {
                var result = statusDataAccess.ExecuteNonQuery(item);

                if(!result.IsSuccess())
                {
                    errorCode = result.Code;

                    if (!result.IsUMOfNonDCS())
                    {
                        errorMessages.Append($"{result.Message}: {refDataService.GetBetTypeName(item.BetType)}<br/>");
                    }
                    else if (errorMessages.Length == 0)
                    {
                        errorMessages.Append(result.Message);
                    }
                }
            }

            return new Result(errorCode, errorCode.Equals(0) ? Resources.StatusUpdatedSuccessfully : errorMessages.ToString());
        }

        private static IEnumerable<UpdateKenoStatusCriteria> CreateUpdateCriteria(
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            foreach (var status in commands)
            {
                yield return new UpdateKenoStatusCriteria(custId, actorId, ip, status);
            }
        }
    }
}
