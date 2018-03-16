namespace Fanex.ProductStatus.Setting.Criteria
{
    using NAP.Framework.Data;

    internal class SelectProductInfoCriteria : CriteriaBase
    {
        public SelectProductInfoCriteria(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; }

        public override string GetSettingKey()
        {
            return "EAS_CustomerSelSportsByProduct";
        }

        public override bool IsValid()
        {
            return ProductId > 0;
        }
    }
}
