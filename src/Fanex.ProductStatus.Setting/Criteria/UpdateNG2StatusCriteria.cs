namespace Fanex.ProductStatus.Setting.Criteria
{
    using System.Collections.Generic;
    using Models;

    internal class UpdateNG2StatusCriteria : UpdateDCSProductStatusCriteria
    {
        public UpdateNG2StatusCriteria(
            int custId,
            int actorId,
            string ip,
            byte settingStatus,
            IEnumerable<ProductStatusCommand> commands)
            : base(custId, actorId, ip, commands, settingStatus)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableGameStatusUpd";
    }
}
