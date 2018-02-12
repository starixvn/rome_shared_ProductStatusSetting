namespace Fanex.ProductStatus.Setting.Criteria
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Models;
    using NAP.Framework.Data;

    internal class UpdateDCSProductStatusCriteria : CriteriaBase
    {
        private readonly int custId;
        private readonly int actorId;
        private readonly string ip;
        private readonly IEnumerable<ProductStatusCommand> productStatusCommands;

        public UpdateDCSProductStatusCriteria(
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            this.custId = custId;
            this.actorId = actorId;
            this.ip = ip;

            productStatusCommands = commands;
        }

        public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatusUpd";

        public override bool IsValid()
            => custId > 0 && actorId > 0;

        public override Hashtable GetSpParams() => new Hashtable
        {
            ["custid"] = custId,
            ["isdisabled"] = GetIsDisabledXmlParam(),
            ["userid"] = actorId,
            ["ip"] = ip
        };

        private string GetIsDisabledXmlParam()
        {
            var builder = new StringBuilder();
            builder.Append("<ROOT>");

            foreach (var item in productStatusCommands)
            {
                builder.Append(BuildProductStatusElement(item));
            }

            builder.Append("</ROOT>");

            return builder.ToString();
        }

        private static string BuildProductStatusElement(ProductStatusCommand command)
        {
            return string.Format(
                "<ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" UpdateAllMasters=\"{3}\" UpdateAllAgents=\"{4}\" UpdateAllMembers=\"{5}\""
                + " DisableAllMasters=\"{6}\" DisableAllAgents=\"{7}\" DisableAllMembers=\"{8}\"/>",
                command.SportType,
                command.BetType,
                Convert.ToInt32(command.Disable),
                Convert.ToInt32(command.UpdateAllMasters),
                Convert.ToInt32(command.UpdateAllAgents),
                Convert.ToInt32(command.UpdateAllMembers),
                Convert.ToInt32(command.DisableAllMasters == true),
                Convert.ToInt32(command.DisableAllAgents == true),
                Convert.ToInt32(command.DisableAllMembers == true));
        }
    }
}
