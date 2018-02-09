namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;

    public class UpdateNumberGameStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateNumberGameStatusCriteria(
            int custId,
            int actorId,
            string ip,
            ProductStatusCommand command)
            : base(custId, actorId, ip, command)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableNumberGame";
    }
}
