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

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }
    }
}
