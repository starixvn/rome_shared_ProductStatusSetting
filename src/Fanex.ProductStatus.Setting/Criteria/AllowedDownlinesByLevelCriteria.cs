namespace Fanex.ProductStatus.Setting.Criteria
{
    using NAP.Framework.Data;

    public class AllowedDownlinesByLevelCriteria : CriteriaBase
    {
        public AllowedDownlinesByLevelCriteria(
            int custId,
            byte custRole,
            bool updateAllMasters,
            bool updateAllAgents,
            bool updateAllMembers)
        {
            CustId = custId;
            CustRole = custRole;

            UpdateAllMasters = updateAllMasters;
            UpdateAllAgents = updateAllAgents;
            UpdateAllMembers = updateAllMembers;
        }

        public int CustId { get; }

        [SpParam(SqlParamName = "RoleId")]
        public byte CustRole { get; }

        public bool UpdateAllMasters { get; }

        public bool UpdateAllAgents { get; }

        public bool UpdateAllMembers { get; }

        public override bool IsValid()
            => CustId > 0 && CustRole > 0;

        public override string GetSettingKey() => "Acc_AdminCheckAllowedDownlinesByLevel";
    }
}
