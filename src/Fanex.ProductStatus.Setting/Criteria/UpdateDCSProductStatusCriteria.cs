using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public class UpdateDCSProductStatusCriteria : CriteriaBase
    {
        private readonly int custId;
        private readonly int userId;
        private readonly string ip;
        private readonly IEnumerable<UpdateProductStatusParam> updateProductStatusParams;

        public UpdateDCSProductStatusCriteria(IEnumerable<UpdateProductStatusParam> statusParams)
        {
            var firstParam = updateProductStatusParams.First();
            custId = firstParam.CustId;
            userId = firstParam.UserId;
            ip = firstParam.IP;

            updateProductStatusParams = statusParams;
        }

        public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatusUpd";

        public override bool IsValid()
            => custId > 0 && userId > 0 && !string.IsNullOrEmpty(ip);

        public override Hashtable GetSpParams() => new Hashtable
        {
            ["custid"] = custId,
            ["isdisabled"] = GetIsDisabledXmlParam(),
            ["userid"] = userId,
            ["ip"] = ip
        };

        private string GetIsDisabledXmlParam()
        {
            var builder = new StringBuilder();
            builder.Append("<ROOT>");

            foreach (var item in updateProductStatusParams)
            {
                builder.Append(BuildProductStatusElement(item));
            }

            builder.Append("</ROOT>");

            return builder.ToString();
        }

        private static string BuildProductStatusElement(UpdateProductStatusParam command)
        {
            return string.Format(
                "<ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" UpdateAllMasters=\"{3}\" UpdateAllAgents=\"{4}\" UpdateAllMembers=\"{5}\" DisableAllMasters=\"{6}\" DisableAllAgents=\"{7}\" DisableAllMembers=\"{8}\"/>",
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
