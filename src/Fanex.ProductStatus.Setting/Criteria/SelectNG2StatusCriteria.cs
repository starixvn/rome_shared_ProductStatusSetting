namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class SelectNG2StatusCriteria : SelectProductStatusCriteria
    {
        public SelectNG2StatusCriteria(int productId, int custId, byte settingStatus)
            : base(productId, custId, settingStatus)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableGameStatusSelect";
    }
}
