namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;

    internal class UpdateColossusBetsStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateColossusBetsStatusCriteria(
            int custId,
            int actorId,
            string ip,
            ProductStatusCommand command)
            : base(custId, actorId, ip, command)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableColossusBet";
    }
}
