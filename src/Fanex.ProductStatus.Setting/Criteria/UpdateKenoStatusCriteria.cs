using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class UpdateKenoStatusCriteria : UpdateBaseProductStatusCriteria
    {
        public UpdateKenoStatusCriteria(ProductStatusCommand command)
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
            BetType = command.BetType;
            UplineId = command.UplineId;
        }

        public int BetType { get; }

        public int UplineId { get; }

        public override string GetSettingKey() => "Acc_AdminUpdDisableKeno";

        public override bool IsValid()
            => base.IsValid() && BetType > 0 && UplineId > 0;
    }
}
