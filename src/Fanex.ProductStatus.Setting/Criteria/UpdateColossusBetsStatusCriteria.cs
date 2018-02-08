namespace Fanex.ProductStatus.Setting.Criteria
{
    using Fanex.ProductStatus.Setting.Models;

    public class UpdateColossusBetsStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateColossusBetsStatusCriteria(ProductStatusCommand command)
            : base(command)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableColossusBet";
    }
}
