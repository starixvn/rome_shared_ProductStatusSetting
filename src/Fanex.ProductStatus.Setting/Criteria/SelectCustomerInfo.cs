namespace Fanex.ProductStatus.Setting.Criteria
{
    using NAP.Framework.Data;

    public class SelectCustomerInfo : CriteriaBase
    {
        public SelectCustomerInfo(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override bool IsValid() => CustId > 0;

        public override string GetSettingKey() => "Age_CustomerSelUserInfo";
    }
}
