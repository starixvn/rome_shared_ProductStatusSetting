using Fanex.NAP.Framework.Data;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public abstract class UpdateBaseProductStatusCriteria : CriteriaBase
    {
        protected UpdateBaseProductStatusCriteria()
        {
        }

        protected UpdateBaseProductStatusCriteria(
            int custId,
            bool disable,
            int userId,
            string ip,
            bool updateAllMasters = false,
            bool disableAllMasters = true,
            bool updateAllAgents = false,
            bool disableAllAgents = true,
            bool updateAllMembers = false,
            bool disableAllMembers = true)
        {
            CustId = custId;
            Disable = disable;
            UserId = userId;
            IP = ip;

            UpdateAllMasters = updateAllMasters;
            DisableAllMasters = disableAllMasters;

            UpdateAllAgents = updateAllAgents;
            DisableAllAgents = disableAllAgents;

            UpdateAllMembers = updateAllMembers;
            DisableAllMembers = disableAllMembers;
        }

        public int CustId { get; }

        public bool Disable { get; }

        public bool UpdateAllMasters { get; }

        public bool DisableAllMasters { get; }

        public bool UpdateAllAgents { get; }

        public bool DisableAllAgents { get; }

        public bool UpdateAllMembers { get; }

        public bool DisableAllMembers { get; }

        public int UserId { get; }

        public string IP { get; }

        public override bool IsValid()
            => CustId > 0 && UserId > 0 && !string.IsNullOrEmpty(IP);
    }
}
