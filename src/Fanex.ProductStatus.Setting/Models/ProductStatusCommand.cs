namespace Fanex.ProductStatus.Setting.Models
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class ProductStatusCommand
    {
        public byte CustRole { get; set; }

        private bool updateAllAgents;
        private bool disableAllAgents;
        private bool updateAllMembers;
        private bool disableAllMembers;

        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool Disable { get; set; }

        public bool UpdateAllMasters { get; set; }

        public bool DisableAllMasters { get; set; }

        public bool UpdateAllAgents
        {
            get
            {
                return DisableAllMasters || updateAllAgents;
            }
            set
            {
                updateAllAgents = value;
            }
        }

        public bool DisableAllAgents
        {
            get
            {
                return DisableAllMasters || disableAllAgents;
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
                return DisableAllMasters || DisableAllAgents || updateAllMembers;
            }
            set
            {
                updateAllMembers = value;
            }
        }

        public bool DisableAllMembers
        {
            get
            {
                return DisableAllMasters || DisableAllAgents || disableAllMembers;
            }
            set
            {
                disableAllMembers = value;
            }
        }

        public bool Valid()
         => CustRole > 0
         && CustRole < 5
         && ValidUpdate[CustRole]();

        public IDictionary<byte, Func<bool>> ValidUpdate
        {
            get
            {
                return new Dictionary<byte, Func<bool>>
                {
                    [RoleIdentity.Super] = ValidSuper,
                    [RoleIdentity.Master] = ValidMaster,
                    [RoleIdentity.Agent] = ValidAgent
                };
            }
        }

        internal bool ValidSuper()
           => !Disable || DisableAllMasters && DisableAllAgents && DisableAllMembers;

        internal bool ValidMaster()
            => !Disable || DisableAllAgents && DisableAllMembers;

        internal bool ValidAgent()
            => !Disable || DisableAllMembers;
    }
}
