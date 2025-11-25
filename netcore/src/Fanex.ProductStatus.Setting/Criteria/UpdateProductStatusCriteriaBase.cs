namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;
    // TODO: Requires Fanex.NAP.Framework.Data migration
    // using NAP.Framework.Data;

    internal abstract class UpdateProductStatusCriteriaBase // : CriteriaBase
    {
        protected UpdateProductStatusCriteriaBase(
            int custId,
            int actorId,
            string ip,
            ProductStatusCommand command)
        {
            CustId = custId;
            ActorId = actorId;
            IP = ip;

            Disable = command.Disable;

            UpdateAllMasters = command.UpdateAllMasters;
            DisableAllMasters = command.DisableAllMasters;

            UpdateAllAgents = command.UpdateAllAgents;
            DisableAllAgents = command.DisableAllAgents;

            UpdateAllMembers = command.UpdateAllMembers;
            DisableAllMembers = command.DisableAllMembers;
        }

        public int CustId { get; }

        public bool Disable { get; }

        public bool UpdateAllMasters { get; }

        public bool? DisableAllMasters { get; }

        public bool UpdateAllAgents { get; }

        public bool? DisableAllAgents { get; }

        public bool UpdateAllMembers { get; }

        public bool? DisableAllMembers { get; }

        // TODO: Requires SpParam attribute from Fanex.NAP.Framework
        // [SpParam(SqlParamName = \"UserId\")]
        public int ActorId { get; }

        public string IP { get; }

        // TODO: Requires CriteriaBase from Fanex.NAP.Framework
        // public override bool IsValid()
        //     => CustId > 0 && ActorId > 0;
    }
}
