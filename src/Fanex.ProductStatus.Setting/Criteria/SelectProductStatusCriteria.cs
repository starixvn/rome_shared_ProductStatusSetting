using Fanex.NAP.Framework.Data;

namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class SelectProductStatusCriteria : CriteriaBase
    {
        public SelectProductStatusCriteria(int productId, int custId)
        {
            ProductId = productId;
            CustId = custId;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public override string GetSettingKey() => "Acc_CustomerSelStatusByProductId";

        public override bool IsValid() => ProductId > 0 && CustId > 0;
    }
}
