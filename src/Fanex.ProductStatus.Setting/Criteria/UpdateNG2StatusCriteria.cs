using System.Collections.Generic;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public class UpdateNG2StatusCriteria : UpdateDCSProductStatusCriteria
    {
        public UpdateNG2StatusCriteria(IEnumerable<UpdateProductStatusParam> statusParams)
            : base(statusParams)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableGameStatusUpd";
    }
}
