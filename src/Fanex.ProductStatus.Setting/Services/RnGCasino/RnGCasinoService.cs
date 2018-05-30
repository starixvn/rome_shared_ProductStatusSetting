namespace Fanex.ProductStatus.Setting.Services
{
    using CasinoLibrary.Model.UserException;
    using CasinoLibrary.SBAgent;

    public interface RnGCasinoService
    {
        void UpdateEnableCasinoByLevelUpdateEnableCasinoByLevel(
            int IBCCustID, 
            bool EnableCasino, 
            bool UpdateAllMasters, 
            bool UpdateAllAgents, 
            bool UpdateAllMembers, 
            bool EnableAllMasters, 
            bool EnableAllAgents, 
            bool EnableAllMembers, 
            string FromIP, 
            int ModifyBy, 
            string ModifyUserName, 
            int FromSystem);
    }

    public class RnGCasinoServiceImpl : RnGCasinoService
    {
        public void UpdateEnableCasinoByLevelUpdateEnableCasinoByLevel(
            int IBCCustID, 
            bool EnableCasino, 
            bool UpdateAllMasters, 
            bool UpdateAllAgents, 
            bool UpdateAllMembers, 
            bool EnableAllMasters, 
            bool EnableAllAgents, 
            bool EnableAllMembers, 
            string FromIP, 
            int ModifyBy, 
            string ModifyUserName, 
            int FromSystem)
        {
            var customer = new Customer();

            var success = customer.UpdateEnableCasinoByLevel(
                IBCCustID, 
                EnableCasino, 
                UpdateAllMasters, 
                UpdateAllAgents, 
                UpdateAllMembers, 
                EnableAllMasters, 
                EnableAllAgents, 
                EnableAllMembers, 
                FromIP, 
                ModifyBy, 
                ModifyUserName, 
                FromSystem);

            if (!success)
            {
                throw (new ErrorException($"UpdateCustEnableCasino({IBCCustID}) on CasinoLibrary is failure."));
            }
        }
    }
}