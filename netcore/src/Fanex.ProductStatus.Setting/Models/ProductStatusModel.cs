namespace Fanex.ProductStatus.Setting.Models
{
    public class ProductStatusModel
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public ProductStatusModel(
            int sportType,
            int parentSportType,
            int betType,
            int parentBetType,
            bool isDisabled,
            bool isDisabledUpline,
            int totalDisabledMasters,
            int totalEnabledMasters,
            int totalDisabledAgents,
            int totalEnabledAgents,
            int totalDisabledMembers,
            int totalEnabledMembers,
            int totalDisabledSuperDirectMembers,
            int totalEnabledSuperDirectMembers,
            int totalDisabledMasterDirectMembers,
            int totalEnabledMasterDirectMembers) : this(sportType, betType)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            IsDisabled = isDisabled;
            IsDisabledUpline = isDisabledUpline;

            TotalDisabledMasters = totalDisabledMasters;
            TotalEnabledMasters = totalEnabledMasters;

            TotalDisabledAgents = totalDisabledAgents;
            TotalEnabledAgents = totalEnabledAgents;

            TotalDisabledMembers = totalDisabledMembers;
            TotalEnabledMembers = totalEnabledMembers;

            TotalDisabledSuperDirectMembers = totalDisabledSuperDirectMembers;
            TotalEnabledSuperDirectMembers = totalEnabledSuperDirectMembers;

            TotalDisabledMasterDirectMembers = totalDisabledMasterDirectMembers;
            TotalEnabledMasterDirectMembers = totalEnabledMasterDirectMembers;

            ParentSportType = parentSportType;
            ParentBetType = parentBetType;
        }

        public ProductStatusModel(int sportType, int betType)
        {
            SportType = sportType;
            BetType = betType;
        }

        public int SportType { get; }

        public int ParentSportType { get; }

        public int BetType { get; }

        public int ParentBetType { get; }

        public bool IsDisabled { get; }

        public bool IsDisabledUpline { get; }

        public int TotalDisabledMasters { get; }

        public int TotalEnabledMasters { get; }

        public int TotalDisabledAgents { get; }

        public int TotalEnabledAgents { get; }

        public int TotalDisabledMembers { get; }

        public int TotalEnabledMembers { get; }

        public int TotalDisabledSuperDirectMembers { get; }

        public int TotalEnabledSuperDirectMembers { get; }

        public int TotalDisabledMasterDirectMembers { get; }

        public int TotalEnabledMasterDirectMembers { get; }

        public string Name { get; set; }
    }
}
