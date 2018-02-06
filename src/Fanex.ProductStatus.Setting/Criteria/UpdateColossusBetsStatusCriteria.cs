using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public class UpdateColossusBetsStatusCriteria : UpdateBaseProductStatusCriteria
    {
        public UpdateColossusBetsStatusCriteria(ProductStatusCommand command)
            : base(
                  command.CustId,
                  command.Disable,
                  command.UserId,
                  command.IP,
                  command.UpdateAllMasters,
                  command.DisableAllMasters,
                  command.UpdateAllAgents,
                  command.DisableAllAgents,
                  command.UpdateAllMembers,
                  command.DisableAllMembers)
        {
        }

        public override string GetSettingKey() => "Acc_AdminUpdDisableColossusBet";
    }
}
