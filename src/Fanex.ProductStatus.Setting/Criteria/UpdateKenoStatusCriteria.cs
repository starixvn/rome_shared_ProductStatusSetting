using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class UpdateKenoStatusCriteria : UpdateBaseProductStatusCriteria
    {
        public UpdateKenoStatusCriteria(
            int custId,
            int userId,
            int uplineId,
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
            BetType = command.BetType;
            UplineId = uplineId;
        }

        public int BetType { get; }

        public int UplineId { get; }

        public override string GetSettingKey() => "Acc_AdminUpdDisableKeno";

        public override bool IsValid() => base.IsValid() && BetType > 0 && UplineId > 0;
    }
}
