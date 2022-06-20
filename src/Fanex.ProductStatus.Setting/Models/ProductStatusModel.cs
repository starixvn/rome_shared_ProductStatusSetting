namespace Fanex.ProductStatus.Setting.Models
{
    public class ProductStatusModel
    {
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
            int totalEnabledMembers) : this(sportType, betType)
        {
            IsDisabled = isDisabled;
            IsDisabledUpline = isDisabledUpline;

            TotalDisabledMasters = totalDisabledMasters;
            TotalEnabledMasters = totalEnabledMasters;

            TotalDisabledAgents = totalDisabledAgents;
            TotalEnabledAgents = totalEnabledAgents;

            TotalDisabledMembers = totalDisabledMembers;
            TotalEnabledMembers = totalEnabledMembers;

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

        public string Name { get; set; }
    }
}
