namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;

    public class UpdateNumberGameStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateNumberGameStatusCriteria(ProductStatusCommand command)
            : base(command)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableNumberGame";
    }
}
