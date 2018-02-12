namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;

    internal class UpdateKenoStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateKenoStatusCriteria(
            int custId,
            int actorId,
            string ip,
            ProductStatusCommand command)
            : base(custId, actorId, ip, command)
        {
            BetType = command.BetType;
        }

        public int BetType { get; }

        public override string GetSettingKey() => "Acc_AdminUpdDisableKeno";

        public override bool IsValid() => base.IsValid() && BetType > 0;
    }
}
