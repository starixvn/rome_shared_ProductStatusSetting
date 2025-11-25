namespace Fanex.ProductStatus.Setting.Criteria
{
    internal class SelectEsportProductStatusCriteria : SelectProductStatusCriteria
    {
        public SelectEsportProductStatusCriteria(int productId, int custId, byte settingStatus)
            : base(productId, custId, settingStatus)
        {
        }

        // TODO: Requires CriteriaBase from Fanex.NAP.Framework
        // public override string GetSettingKey() => "Acc_Admin_DisableNewProductStatusSelect_Esports";
    }
}
