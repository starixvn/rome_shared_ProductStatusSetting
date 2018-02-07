using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public class UpdateGoldDeluxeStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateGoldDeluxeStatusCriteria(UpdateProductStatusParam statusParam)
            : base(statusParam)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableGoldDeluxe";
    }
}
