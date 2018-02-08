namespace Fanex.ProductStatus.Setting.Criteria
{
    using System.Collections.Generic;
    using Models;

    public class UpdateNG2StatusCriteria : UpdateDCSProductStatusCriteria
    {
        public UpdateNG2StatusCriteria(IEnumerable<ProductStatusCommand> command)
            : base(command)
        {
        }

        public override string GetSettingKey() => "Acc_Admin_DisableGameStatusUpd";
    }
}
