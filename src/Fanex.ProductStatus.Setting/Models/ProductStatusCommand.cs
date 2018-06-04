namespace Fanex.ProductStatus.Setting.Models
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class ProductStatusCommand
    {
        private bool updateAllAgents;

        private bool updateAllMembers;

        public byte CustRole { get; set; }

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

        public bool DisableAllAgents { get; set; }

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

        public bool DisableAllMembers { get; set; }

        public bool Valid() => CustRole > 0 && CustRole < 5 && ValidUpdateByLevel[CustRole]();

        internal IDictionary<byte, Func<bool>> ValidUpdateByLevel
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
