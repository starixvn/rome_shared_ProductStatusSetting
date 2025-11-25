namespace Fanex.ProductStatus.Setting.Criteria
{
    // TODO: Requires Fanex.NAP.Framework.Data migration
    // using NAP.Framework.Data;

    public class SelectProductProfileByGroupCriteria // : CriteriaBase
    {
        public SelectProductProfileByGroupCriteria(int groupId, int groupTypeId, int systemId, byte settingStatus, byte mode = 2)
        {
            GroupId = groupId;
            GroupTypeId = groupTypeId;
            SystemId = systemId;
            SettingStatus = settingStatus;
            Mode = mode;
        }

        public int GroupId { get; }

        public int GroupTypeId { get; }

        public int SystemId { get; }

        public byte SettingStatus { get; }

        public byte Mode { get; }

        // TODO: Requires CriteriaBase from Fanex.NAP.Framework
        // public override string GetSettingKey() => "Acc_LiteMode_CustSetting_ProductProfileSelect";

        // public override bool IsValid() => true;
    }
}
