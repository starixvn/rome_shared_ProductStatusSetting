namespace Fanex.ProductStatus.Setting.Services
{
    using AppResources;
    using RemotingRef;

    public interface RefDataService
    {
        string GetBetTypeName(int betTypeId);

        string GetSportName(int sportTypeId);

        string GetProductName(int productId);
    }

    public class RefDataServiceImpl : RefDataService
    {
        private const int FirstBetTypeIdOfAllbets = 2401;
        private const string DefaultLanguage = "en";
        private const string ProductRefId = "Product";

        public string GetBetTypeName(int betTypeId)
        {
            if (betTypeId == FirstBetTypeIdOfAllbets)
            {
                return Resources.BetType_2401;
            }

            return RefData.GetBetTypeName(betTypeId.ToString(), DefaultLanguage);
        }

        public string GetSportName(int sportTypeId)
            => RefData.GetSportName(sportTypeId.ToString(), DefaultLanguage);

        public string GetProductName(int productId)
            => RefData.GetResourceDataByRefIdAndResourceId(ProductRefId, productId.ToString(), DefaultLanguage, string.Empty);
    }
}
