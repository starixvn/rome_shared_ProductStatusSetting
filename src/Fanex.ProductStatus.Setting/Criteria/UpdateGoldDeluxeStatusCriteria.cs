namespace Fanex.ProductStatus.Setting.Criteria
{
    using Models;

    internal class UpdateGoldDeluxeStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateGoldDeluxeStatusCriteria(
            int custId,
            int actorId,
            string ip,
            ProductStatusCommand command)
            : base(custId, actorId, ip, command)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableGoldDeluxe";
    }
}
