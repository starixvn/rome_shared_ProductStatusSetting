namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class SelectDCSProductStatusCriteria : SelectProductStatusCriteria
    {
        public int? SettingType { get; set; }

        public SelectDCSProductStatusCriteria(int productId, int custId, byte settingStatus, int? settingType = null)
            : base(productId, custId, settingStatus)
        {
            this.SettingType = settingType;
        }

        // TODO: Requires CriteriaBase from Fanex.NAP.Framework
        // public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatusSelect";
    }
}
