namespace Fanex.ProductStatus.Setting.Services.CommandServices
{
    using System.Collections.Generic;
    using System.Linq;
    using CasinoLibrary.Model.UserException;
    using CasinoLibrary.SBAgent;
    using Criteria;
    using DataAccess;
    using Models;

    internal class CasinoProductStatusCommandService : ProductStatusCommandService
    {
        private const int FromSystem = 3;

        private readonly ProductStatusDataAccess statusDataAccess;

        public CasinoProductStatusCommandService(ProductStatusDataAccess statusDataAccess = null)
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
            var command = commands.First();
            var result = UpdateEnableCasinoByLevel(
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
                FromSystem);

            if (result.IsSuccess())
            {
                var criteria = new UpdateCasinoStatusCriteria(custId, actorId, ip, command);

                result = statusDataAccess.ExecuteNonQuery(criteria);
            }

            return result;
        }

        private Result UpdateEnableCasinoByLevel(
            int custId,
            byte custRole,
            bool enableCasino,
            bool updateAllMasters,
            bool updateAllAgents,
            bool updateAllMembers,
            bool enableAllMasters,
            bool enableAllAgents,
            bool enableAllMembers,
            string fromIP,
            int modifyBy,
            string modifyUserName,
            int fromSystem)
        {
            var result = AdminCheckAllowedDownlinesByLevel(
                custId,
                custRole,
                updateAllMasters,
                updateAllAgents,
                updateAllMembers);

            if (result.IsSuccess())
            {
                var customer = new Customer();

                if (!customer.UpdateEnableCasinoByLevel(
                    custId,
                    enableCasino,
                    updateAllMasters,
                    updateAllAgents,
                    updateAllMembers,
                    enableAllMasters,
                    enableAllAgents,
                    enableAllMembers,
                    fromIP,
                    modifyBy,
                    modifyUserName,
                    fromSystem))
                {
                    throw (new ErrorException($"UpdateCustEnableCasino({custId}) on CasinoLibrary is failure."));
                }
            }

            return result;
        }

        private Result AdminCheckAllowedDownlinesByLevel(
             int custId,
             byte custRole,
             bool updateAllMasters,
             bool updateAllAgents,
             bool updateAllMembers)
             => statusDataAccess.ExecuteNonQuery(
                 new AllowedDownlinesByLevelCriteria(
                     custId,
                     custRole,
                     updateAllMasters,
                     updateAllAgents,
                     updateAllMembers));
    }
}
