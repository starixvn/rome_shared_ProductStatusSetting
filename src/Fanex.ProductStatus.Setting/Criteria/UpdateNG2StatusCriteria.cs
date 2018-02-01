using System.Collections.Generic;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class UpdateNG2StatusCriteria : UpdateDCSProductStatusCriteria
    {
        public UpdateNG2StatusCriteria(
            int custId,
            int userId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
            : base(custId, userId, ip, commands)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableGameStatusUpd";
    }
}
