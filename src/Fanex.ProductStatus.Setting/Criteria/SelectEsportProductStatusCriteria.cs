namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class SelectEsportProductStatusCriteria : SelectProductStatusCriteria
    {
        public SelectEsportProductStatusCriteria(int productId, int custId)
            : base(productId, custId)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatusSelect_Esports";
    }
}
