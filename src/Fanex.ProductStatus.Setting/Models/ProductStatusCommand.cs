namespace Fanex.ProductStatus.Setting.Models
{
    public class ProductStatusCommand
    {
        private bool updateAllAgents;
        private bool? disableAllAgents;
        private bool updateAllMembers;
        private bool? disableAllMembers;

        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool Disable { get; set; }

        public bool UpdateAllMasters { get; set; }

        public bool? DisableAllMasters { get; set; }

        public bool UpdateAllAgents
        {
            get
            {
                return DisableAllMasters == true || updateAllAgents;
            }
            set
            {
                updateAllAgents = value;
            }
        }

        public bool? DisableAllAgents
        {
            get
            {
                return DisableAllMasters == true ? true : disableAllAgents;
            }
            set
            {
                disableAllAgents = value;
            }
        }

        public bool UpdateAllMembers
        {
            get
            {
                return DisableAllMasters == true || DisableAllAgents == true || updateAllMembers;
            }
            set
            {
                updateAllMembers = value;
            }
        }

        public bool? DisableAllMembers
        {
            get
            {
                return DisableAllMasters == true || DisableAllAgents == true
                    ? true
                    : disableAllMembers;
            }
            set
            {
                disableAllMembers = value;
            }
        }
    }
}
