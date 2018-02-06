namespace Fanex.ProductStatus.Setting.Criteria
{
    public class SelectNG2StatusCriteria : SelectProductStatusCriteria
    {
        public SelectNG2StatusCriteria(int productId, int custId)
            : base(productId, custId)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableGameStatusSelect";
    }
}
