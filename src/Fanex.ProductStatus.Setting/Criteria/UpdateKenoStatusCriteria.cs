using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Criteria
{
    public class UpdateKenoStatusCriteria : UpdateProductStatusCriteriaBase
    {
        public UpdateKenoStatusCriteria(UpdateProductStatusParam statusParam)
            : base(statusParam)
        {
            BetType = statusParam.BetType;
            UplineId = statusParam.UplineId;
        }

        public int BetType { get; }

        public int UplineId { get; }

        public override string GetSettingKey() => "Acc_AdminUpdDisableKeno";

        public override bool IsValid()
            => base.IsValid() && BetType > 0 && UplineId > 0;
    }
}
