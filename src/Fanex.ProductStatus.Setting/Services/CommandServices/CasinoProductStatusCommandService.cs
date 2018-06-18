namespace Fanex.ProductStatus.Setting.Services.CommandServices
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using DataAccess;
    using Models;

    internal class CasinoProductStatusCommandService : ProductStatusCommandService
    {
        private const int FromSystem = 3;
        private const int UnsyncedRnGCasinoCode = 109;
        private const string UnsyncedRnGCasinoMessage = "Product setting is not synchronized";

        private readonly ProductStatusDataAccess statusDataAccess;
        private readonly RngCasinoAPIWrapper rngCasinoAPIWrapper;

        public CasinoProductStatusCommandService(
            ProductStatusDataAccess statusDataAccess = null,
            RngCasinoAPIWrapper rngCasinoAPIWrapper = null)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
            this.rngCasinoAPIWrapper = rngCasinoAPIWrapper ?? new RngCasinoAPIWrapperImpl();
        }

        public Result UpdateProductStatus(
            int productId,
            int custId,
            byte custRole,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
            => statusDataAccess.SyncedRnGCasino(new SelectCustomerInfo(custId)) ?
            UpdateEnableCasinoByLevel(custId, custRole, actorId, actorName, ip, commands)
            : new Result(UnsyncedRnGCasinoCode, UnsyncedRnGCasinoMessage);

        internal Result UpdateEnableCasinoByLevel(
            int custId,
            byte custRole,
            int actorId,
            string actorName,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            var command = commands.First();

            var result = SynchronizeStatus(
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
                return statusDataAccess.ExecuteNonQuery(new UpdateCasinoStatusCriteria(custId, actorId, ip, command));
            }

            return result;
        }

        internal Result SynchronizeStatus(
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
            var checkAllowDownlinesResult = statusDataAccess.ExecuteNonQuery(
                 new AllowedDownlinesByLevelCriteria(
                     custId,
                     custRole,
                     updateAllMasters,
                     updateAllAgents,
                     updateAllMembers));

            if (checkAllowDownlinesResult.IsSuccess())
            {
                rngCasinoAPIWrapper.UpdateEnableCasinoByLevel(
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
                    fromSystem);
            }

            return checkAllowDownlinesResult;
        }
    }
}
