namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;

    public class UpdateGoldDeluxeStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateGoldDeluxeStatusCriteria(ProductStatusCommand command)
            : base(command)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableGoldDeluxe";
    }
}
