namespace Fanex.ProductStatus.Setting.Criteria
{
    using System;
    using System.Collections;
    using Models;

    public class UpdateCasinoStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateCasinoStatusCriteria(
            int custId,
            int actorId,
            string ip,
            ProductStatusCommand command)
            : base(custId, actorId, ip, command)
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
