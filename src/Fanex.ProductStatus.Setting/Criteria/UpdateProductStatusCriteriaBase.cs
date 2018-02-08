namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;
    using NAP.Framework.Data;

    public abstract class UpdateProductStatusCriteriaBase : CriteriaBase
    {
        protected UpdateProductStatusCriteriaBase(ProductStatusCommand command)
        {
            CustId = command.CustId;
            ActorId = command.ActorId;
            IP = command.IP;

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

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; }

        public string IP { get; }

        public override bool IsValid()
            => CustId > 0
            && ActorId > 0
            && !string.IsNullOrEmpty(IP);
    }
}
