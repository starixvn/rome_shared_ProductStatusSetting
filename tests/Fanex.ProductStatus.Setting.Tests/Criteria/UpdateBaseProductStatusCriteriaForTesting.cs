using System;
using Fanex.ProductStatus.Setting.Criteria;

namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    internal class UpdateBaseProductStatusCriteriaForTesting : UpdateBaseProductStatusCriteria
    {
        public UpdateBaseProductStatusCriteriaForTesting()
        {
        }

        public UpdateBaseProductStatusCriteriaForTesting(
            int custId,
            bool disable,
            int userId,
            string ip,
            bool updateAllMasters = false,
            bool disableAllMasters = true,
            bool updateAllAgents = false,
            bool disableAllAgents = true,
            bool updateAllMembers = false,
            bool disableAllMembers = true)
            : base(
                  custId,
                  disable,
                  userId,
                  ip,
                  updateAllMasters,
                  disableAllMasters,
                  updateAllAgents,
                  disableAllAgents,
                  updateAllMembers,
                  disableAllMembers)
        {
        }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }
    }
}
