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

            Master = new DownlineStatus(isDisabledUpline, totalDisabledMasters, totalEnabledMasters);
            Agent = new DownlineStatus(isDisabledUpline, totalDisabledAgents, totalEnabledAgents);
            Member = new DownlineStatus(isDisabledUpline, totalDisabledMembers, totalEnabledMembers);
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
