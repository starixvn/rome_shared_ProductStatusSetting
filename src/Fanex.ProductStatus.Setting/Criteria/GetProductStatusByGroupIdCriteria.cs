using Fanex.NAP.Framework.Data;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public class GetProductStatusByGroupIdCriteria : CriteriaBase
    {
        public GetProductStatusByGroupIdCriteria(int custId, int groupId, byte settingStatus)
        {
            CustId = custId;
            GroupId = groupId;
            SettingStatus = settingStatus;
        }

        public int CustId { get; }

        public int GroupId { get; }

        public byte SettingStatus { get; }

        public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatus_160_Select";
    }
}