namespace Fanex.ProductStatus.Setting.Services.CommandServices
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using DataAccess;
    using Models;
    using NAP.Framework.Data;

    internal class KenoProductStatusCommandService : ProductStatusCommandService
    {
        private readonly ProductStatusDataAccess statusDataAccess;

        public KenoProductStatusCommandService(ProductStatusDataAccess statusDataAccess = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
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
            IList<Result> results = new List<Result>();
            var criteria = CreateUpdateCriteria(custId, actorId, ip, commands);

            foreach (var item in criteria)
            {
                results.Add(statusDataAccess.ExecuteNonQuery(item));
            }

            var errorResult = results.FirstOrDefault(r => !r.IsSuccess());

            return errorResult ?? results[0];
        }

        private static IEnumerable<ICriteria> CreateUpdateCriteria(
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
