namespace Fanex.ProductStatus.Setting.Models
{
    public class UpdateProductStatusParam
    {
        public UpdateProductStatusParam(
            int custId,
            int sportType,
            int betType,
            bool disable,
            bool updateAllMasters,
            bool? disableAllMasters,
            bool updateAllAgents,
            bool? disableAllAgents,
            bool updateAllMembers,
            bool? disableAllMembers,
            int uplineId,
            int userId,
            string ip)
        {
            CustId = custId;
            SportType = sportType;
            BetType = betType;

            Disable = disable;

            UpdateAllMasters = updateAllMasters;
            DisableAllMasters = disableAllMasters;

            UpdateAllAgents = updateAllAgents;
            DisableAllAgents = disableAllAgents;

            UpdateAllMembers = updateAllMembers;
            DisableAllMembers = disableAllMembers;

            UplineId = uplineId;
            UserId = userId;
            IP = ip;
        }

        public int CustId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public bool Disable { get; }

        public bool UpdateAllMasters { get; }

        public bool? DisableAllMasters { get; }

        public bool UpdateAllAgents { get; }

        public bool? DisableAllAgents { get; }

        public bool UpdateAllMembers { get; }

        public bool? DisableAllMembers { get; }

        public int UplineId { get; }

        public int UserId { get; }

        public string IP { get; }
    }
}
