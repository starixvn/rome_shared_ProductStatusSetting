namespace Fanex.ProductStatus.Setting.Criteria
{
    public class SelectDCSProductStatusCriteria : SelectProductStatusCriteria
    {
        public SelectDCSProductStatusCriteria(int productId, int custId)
            : base(productId, custId)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatusSelect";
    }
}
