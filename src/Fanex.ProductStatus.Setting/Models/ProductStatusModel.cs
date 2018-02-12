using Fanex.ProductStatus.Setting.Shared;

namespace Fanex.ProductStatus.Setting.Models
{
    public class ProductStatusModel
    {
        public ProductStatusModel(
            int sportType,
            int betType,
            bool isDisabled,
            bool isDisabledUpline,
            int totalDisabledMasters,
            int totalEnabledMasters,
            int totalDisabledAgents,
            int totalEnabledAgents,
            int totalDisabledMembers,
            int totalEnabledMembers,
            byte custRoleId)
        {
            SportType = sportType;
            BetType = betType;
            IsDisabled = isDisabledUpline ? isDisabledUpline : isDisabled;
            IsDisabledUpline = isDisabledUpline;

            switch (custRoleId)
            {
                case RoleIdentity.Super:
                    Master = new DownlineStatus(IsDisabled, totalDisabledMasters, totalEnabledMasters);
                    Agent = new DownlineStatus(IsDisabled || totalEnabledMasters == 0, totalDisabledAgents, totalEnabledAgents);
                    Member = new DownlineStatus(IsDisabled || totalEnabledAgents == 0, totalDisabledMembers, totalEnabledMembers);
                    break;

                case RoleIdentity.Master:
                    Agent = new DownlineStatus(IsDisabled, totalDisabledAgents, totalEnabledAgents);
                    Member = new DownlineStatus(IsDisabled || totalEnabledAgents == 0, totalDisabledMembers, totalEnabledMembers);
                    break;

                case RoleIdentity.Agent:
                    Member = new DownlineStatus(IsDisabled, totalDisabledMembers, totalEnabledMembers);
                    break;

                default:
                    Master = null;
                    Agent = null;
                    Member = null;
                    break;
            }
        }

        public int SportType { get; }

        public int BetType { get; }

        public bool IsDisabled { get; }

        public bool IsDisabledUpline { get; }

        public DownlineStatus Master { get; }

        public DownlineStatus Agent { get; }

        public DownlineStatus Member { get; }

        public int ProductId { get; set; }

        public string Name { get; set; }
    }
}
