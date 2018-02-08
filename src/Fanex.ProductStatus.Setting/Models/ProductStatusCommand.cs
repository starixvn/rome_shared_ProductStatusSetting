namespace Fanex.ProductStatus.Setting.Models
{
    public class ProductStatusCommand
    {
        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool Disable { get; set; }

        public bool UpdateAllMasters { get; set; }

        public bool? DisableAllMasters { get; set; }

        public bool UpdateAllAgents { get; set; }

        public bool? DisableAllAgents { get; set; }

        public bool UpdateAllMembers { get; set; }

        public bool? DisableAllMembers { get; set; }

        public int CustId { get; set; }

        public int ActorId { get; set; }

        public string IP { get; set; }
    }
}
