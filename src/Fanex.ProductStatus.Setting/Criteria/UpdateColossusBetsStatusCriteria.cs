using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public class UpdateColossusBetsStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateColossusBetsStatusCriteria(UpdateProductStatusParam statusParam)
            : base(statusParam)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableColossusBet";
    }
}
