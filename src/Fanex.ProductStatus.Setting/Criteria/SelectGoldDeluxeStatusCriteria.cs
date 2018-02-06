namespace Fanex.ProductStatus.Setting.Criteria
{
    public class SelectGoldDeluxeStatusCriteria : SelectProductStatusCriteria
    {
        public SelectGoldDeluxeStatusCriteria(int productId, int custId)
            : base(productId, custId)
        {
        }

        public override string GetSettingKey() => "Acc_CustomerSelStatusGoldDeluxe";
    }
}
