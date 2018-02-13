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
            int totalEnabledMembers)
        {
            SportType = sportType;
            BetType = betType;
            IsDisabled = isDisabled;
            IsDisabledUpline = isDisabledUpline;

            TotalDisabledMasters = totalDisabledMasters;
            TotalEnabledMasters = totalEnabledMasters;

            TotalDisabledAgents = totalDisabledAgents;
            TotalEnabledAgents = totalEnabledAgents;

            TotalDisabledMembers = totalDisabledMembers;
            TotalEnabledMembers = totalEnabledMembers;
        }

        public int SportType { get; }

        public int BetType { get; }

        public bool IsDisabled { get; }

        public bool IsDisabledUpline { get; }

        public int TotalDisabledMasters { get; }

        public int TotalEnabledMasters { get; }

        public int TotalDisabledAgents { get; }

        public int TotalEnabledAgents { get; }

        public int TotalDisabledMembers { get; }

        public int TotalEnabledMembers { get; }

        public int ProductId { get; set; }

        public string Name { get; set; }
    }
}
