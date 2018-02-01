using System.Collections;
using System.Collections.Generic;
using System.Text;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class UpdateDCSProductStatusCriteria : UpdateBaseProductStatusCriteria
    {
        public UpdateDCSProductStatusCriteria(
            int custId,
            int userId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
            : base(custId, userId, ip)
        {
            ProductStatusCommands = commands;
        }

        public IEnumerable<ProductStatusCommand> ProductStatusCommands { get; }

        public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatusUpd";

        public override Hashtable GetSpParams()
        {
            return new Hashtable
            {
                ["custid"] = CustId,
                ["isdisabled"] = GetIsDisabledXmlParam(),
                ["userid"] = UserId,
                ["ip"] = IP
            };
        }

        private string GetIsDisabledXmlParam()
        {
            var builder = new StringBuilder();
            builder.Append("<ROOT>");

            foreach (var item in ProductStatusCommands)
            {
                builder.Append(BuildProductStatusElement(item));
            }

            builder.Append("</ROOT>");

            return builder.ToString();
        }

        private static string BuildProductStatusElement(ProductStatusCommand command)
        {
            return string.Format(
                "<ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" UpdateAllMasters=\"{3}\" UpdateAllAgents=\"{4}\" UpdateAllMembers=\"{5}\" DisableAllMasters=\"{6}\" DisableAllAgents=\"{7}\" DisableAllMembers=\"{8}\"/>",
                command.SportType,
                command.BetType,
                BooleanToSQLBit(command.Disable),
                BooleanToSQLBit(command.UpdateAllMasters),
                BooleanToSQLBit(command.UpdateAllAgents),
                BooleanToSQLBit(command.UpdateAllMembers),
                BooleanToSQLBit(command.DisableAllMasters),
                BooleanToSQLBit(command.DisableAllAgents),
                BooleanToSQLBit(command.DisableAllMembers));
        }

        private static int BooleanToSQLBit(bool value)
        {
            return value ? 1 : 0;
        }
    }
}
