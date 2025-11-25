namespace Fanex.ProductStatus.Setting.Criteria
{
    // TODO: Requires Fanex.NAP.Framework.Data migration
    // using NAP.Framework.Data;

    internal class SelectProductStatusCriteria // : CriteriaBase
    {
        public SelectProductStatusCriteria(int productId, int custId, byte settingStatus)
        {
            ProductId = productId;
            CustId = custId;
            SettingStatus = settingStatus;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public byte SettingStatus { get; }

        // TODO: Requires CriteriaBase from Fanex.NAP.Framework
        // public override string GetSettingKey() => "Acc_AdminSelStatusByProductId";

        // public override bool IsValid() => ProductId > 0 && CustId > 0;
    }
}
