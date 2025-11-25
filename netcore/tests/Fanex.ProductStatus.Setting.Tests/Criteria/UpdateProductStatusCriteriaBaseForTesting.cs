namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using System;
    using Setting.Criteria;
    using Setting.Models;

    internal class UpdateProductStatusCriteriaBaseForTesting : UpdateProductStatusCriteriaBase
    {
        public UpdateProductStatusCriteriaBaseForTesting(
            int custId,
            int actorId,
            string ip,
            ProductStatusCommand command)
            : base(custId, actorId, ip, command)
        {
        }

        // TODO: GetSettingKey was commented in base class - no longer available to override
        // public override string GetSettingKey()
        // {
        //     throw new NotImplementedException();
        // }
    }
}
