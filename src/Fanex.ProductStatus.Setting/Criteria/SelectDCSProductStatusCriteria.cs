namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class SelectDCSProductStatusCriteria : SelectProductStatusCriteria
    {
        public SelectDCSProductStatusCriteria(int productId, int custId, byte settingStatus)
            : base(productId, custId, settingStatus)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatusSelect";
    }
}
