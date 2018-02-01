using System;
using System.Collections;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class UpdateCasinoStatusCriteria : UpdateBaseProductStatusCriteria
    {
        public UpdateCasinoStatusCriteria(
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

        public bool SyncCS { get; } = true;

        public override string GetSettingKey() => "Acc_AdminUpdCSStatus_Cust";

        public override Hashtable GetSpParams()
        {
            var inputParams = base.GetSpParams();

            var disabledCS = Convert.ToBoolean(inputParams["disable"]);
            inputParams.Remove("disable");
            inputParams.Add("disabledcs", disabledCS);

            return inputParams;
        }
    }
}
