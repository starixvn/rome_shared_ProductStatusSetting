namespace Fanex.ProductStatus.Setting.Services
{
    using CasinoLibrary.Model.UserException;
    using CasinoLibrary.SBAgent;
    using Criteria;
    using DataAccess;
    using Models;

    public interface RnGCasinoService
    {
        Result AdminCheckAllowedDownlinesByLevel(
            int custId,
            byte custRole,
            bool updateAllMasters,
            bool updateAllAgents,
            bool updateAllMembers);

        void UpdateEnableCasinoByLevelUpdateEnableCasinoByLevel(
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
            int fromSystem);
    }

    public class RnGCasinoServiceImpl : RnGCasinoService
    {
        private readonly ProductStatusDataAccess statusDataAccess;
        public RnGCasinoServiceImpl(ProductStatusDataAccess statusDataAccess)
        {
            this.statusDataAccess = statusDataAccess ?? new ProductStatusDataAccessImpl();
        }

       public Result AdminCheckAllowedDownlinesByLevel(
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

        public void UpdateEnableCasinoByLevelUpdateEnableCasinoByLevel(
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
            if (AdminCheckAllowedDownlinesByLevel(
                custId,
                custRole,
                updateAllMasters,
                updateAllAgents,
                updateAllMembers).IsSuccess())
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
        }
    }
}