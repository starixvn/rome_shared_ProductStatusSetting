namespace Fanex.ProductStatus.Setting.Models
{
    public class ProductStatusCommand
    {
        private ProductStatusCommand(
            int sportType,
            int betType,
            bool disable,
            bool updateAllMasters = false,
            bool disableAllMasters = true,
            bool updateAllAgents = false,
            bool disableAllAgents = true,
            bool updateAllMembers = false,
            bool disableAllMembers = true)
        {
            SportType = sportType;
            BetType = betType;
            Disable = disable;

            UpdateAllMasters = updateAllMasters;
            DisableAllMasters = disableAllMasters;

            UpdateAllAgents = updateAllAgents;
            DisableAllAgents = disableAllAgents;

            UpdateAllMembers = updateAllMembers;
            DisableAllMembers = disableAllMembers;
        }

        public int SportType { get; }

        public int BetType { get; }

        public bool Disable { get; }

        public bool UpdateAllMasters { get; }

        public bool DisableAllMasters { get; }

        public bool UpdateAllAgents { get; }

        public bool DisableAllAgents { get; }

        public bool UpdateAllMembers { get; }

        public bool DisableAllMembers { get; }

        public static ProductStatusCommand UpdateMember(
            int sportType,
            int betType,
            bool disable)
        {
            return new ProductStatusCommand(sportType, betType, disable);
        }

        public static ProductStatusCommand DisableAgent(
            int sportType,
            int betType)
        {
            return new ProductStatusCommand(
                sportType,
                betType,
                true,
                updateAllMembers: true,
                disableAllMembers: true);
        }

        public static ProductStatusCommand EnableAgent(
            int sportType,
            int betType,
            bool updateAllMembers,
            bool disableAllMembers)
        {
            return new ProductStatusCommand(
                sportType,
                betType,
                false,
                updateAllMembers,
                disableAllMembers);
        }

        public static ProductStatusCommand DisableMaster(
            int sportType,
            int betType)
        {
            return new ProductStatusCommand(
                   sportType,
                   betType,
                   true,
                   updateAllAgents: true,
                   disableAllAgents: true,
                   updateAllMembers: true,
                   disableAllMembers: true);
        }

        public static ProductStatusCommand EnableMaster(
            int sportType,
            int betType,
            bool updateAllAgents,
            bool disableAllAgents,
            bool updateAllMembers,
            bool disableAllMembers)
        {
            return new ProductStatusCommand(
                sportType,
                betType,
                false,
                updateAllAgents: updateAllAgents,
                disableAllAgents: disableAllAgents,
                updateAllMembers: updateAllMembers,
                disableAllMembers: disableAllMembers);
        }

        public static ProductStatusCommand DisableSuper(
            int sportType,
            int betType)
        {
            return new ProductStatusCommand(
                   sportType,
                   betType,
                   true,
                   updateAllMasters: true,
                   disableAllMasters: true,
                   updateAllAgents: true,
                   disableAllAgents: true,
                   updateAllMembers: true,
                   disableAllMembers: true);
        }

        public static ProductStatusCommand EnableSuper(
            int sportType,
            int betType,
            bool updateAllMasters,
            bool disableAllMasters,
            bool updateAllAgents,
            bool disableAllAgents,
            bool updateAllMembers,
            bool disableAllMembers)
        {
            return new ProductStatusCommand(
                   sportType,
                   betType,
                   false,
                   updateAllMasters: updateAllMasters,
                   disableAllMasters: disableAllMasters,
                   updateAllAgents: updateAllAgents,
                   disableAllAgents: disableAllAgents,
                   updateAllMembers: updateAllMembers,
                   disableAllMembers: disableAllMembers);
        }
    }
}
