namespace Fanex.ProductStatus.Setting.Services.CommandServices
{
    using System;
    using System.Diagnostics;
    using CasinoLibrary.SBAgent;
    using Logging.Extensions.Web;

    public interface RngCasinoAPIWrapper
    {
        void UpdateEnableCasinoByLevel(
            int custId,
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
            int fromSystem);
    }

    public class RngCasinoAPIWrapperImpl : RngCasinoAPIWrapper
    {
        private readonly Customer customer;

        public RngCasinoAPIWrapperImpl()
        {
            customer = new Customer();
        }

        public void UpdateEnableCasinoByLevel(
            int custId,
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
                ExceptionHandler.Handle(new Exception($"UpdateCustEnableCasino({custId}) on CasinoLibrary is failure."), TraceEventType.Warning);
            }
        }
    }
}
