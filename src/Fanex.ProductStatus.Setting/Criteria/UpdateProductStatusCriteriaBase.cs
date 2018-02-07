using Fanex.NAP.Framework.Data;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public abstract class UpdateProductStatusCriteriaBase : CriteriaBase
    {
        protected UpdateProductStatusCriteriaBase(UpdateProductStatusParam statusParam)
        {
            CustId = statusParam.CustId;
            UserId = statusParam.UserId;
            IP = statusParam.IP;

            Disable = statusParam.Disable;

            UpdateAllMasters = statusParam.UpdateAllMasters;
            DisableAllMasters = statusParam.DisableAllMasters;

            UpdateAllAgents = statusParam.UpdateAllAgents;
            DisableAllAgents = statusParam.DisableAllAgents;

            UpdateAllMembers = statusParam.UpdateAllMembers;
            DisableAllMembers = statusParam.DisableAllMembers;
        }

        public int CustId { get; }

        public bool Disable { get; }

        public bool UpdateAllMasters { get; }

        public bool? DisableAllMasters { get; }

        public bool UpdateAllAgents { get; }

        public bool? DisableAllAgents { get; }

        public bool UpdateAllMembers { get; }

        public bool? DisableAllMembers { get; }

        public int UserId { get; }

        public string IP { get; }

        public override bool IsValid()
            => CustId > 0 && UserId > 0 && !string.IsNullOrEmpty(IP);
    }
}
