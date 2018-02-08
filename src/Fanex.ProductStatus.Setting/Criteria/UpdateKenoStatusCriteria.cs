namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;

    public class UpdateKenoStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateKenoStatusCriteria(ProductStatusCommand command)
            : base(command)
        {
            BetType = command.BetType;
        }

        public int BetType { get; }

        public override string GetSettingKey() => "Acc_AdminUpdDisableKeno";

        public override bool IsValid() => base.IsValid() && BetType > 0;
    }
}
