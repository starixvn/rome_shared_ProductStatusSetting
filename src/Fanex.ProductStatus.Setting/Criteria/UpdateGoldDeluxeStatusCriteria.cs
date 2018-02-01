using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class UpdateGoldDeluxeStatusCriteria : UpdateBaseProductStatusCriteria
    {
        public UpdateGoldDeluxeStatusCriteria(
            int custId,
            int userId,
            string ip,
            ProductStatusCommand command)
            : base(
                  custId,
                  command.Disable,
                  userId,
                  ip,
                  command.UpdateAllMasters,
                  command.DisableAllMasters,
                  command.UpdateAllAgents,
                  command.DisableAllAgents,
                  command.UpdateAllMembers,
                  command.DisableAllMembers)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableGoldDeluxe";
    }
}
