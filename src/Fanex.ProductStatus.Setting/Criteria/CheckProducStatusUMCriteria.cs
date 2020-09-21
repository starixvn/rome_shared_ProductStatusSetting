namespace Fanex.ProductStatus.Setting.Criteria
{
    using Fanex.NAP.Framework.Data;
    using Fanex.ProductStatus.Setting.Shared;

    internal class CheckProducStatusUMCriteria : CriteriaBase
    {
        public CheckProducStatusUMCriteria(int productId, int custId)
            : this(productId, custId.ToString())
        {
        }

        public CheckProducStatusUMCriteria(int productId, string custIds)
        {
            ProductId = productId == ProductIdentity.NumberGame
                ? ProductIdentity.Sportsbook
                : productId;
            CustIds = custIds;
        }

        public int ProductId { get; }

        public string CustIds { get; }

        public override string GetSettingKey()
            => "Acc_CheckUM_MultipleCustProductStatus";

        public override bool IsValid()
            => ProductId > 0
            && !string.IsNullOrWhiteSpace(CustIds);
    }
}
