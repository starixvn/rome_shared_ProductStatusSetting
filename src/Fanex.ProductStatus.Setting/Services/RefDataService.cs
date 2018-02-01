using RemotingRef;

namespace Fanex.ProductStatus.Setting.Services
{
    internal class RefDataService : IRefDataService
    {
        private const string DefaultLanguage = "en";
        private const string ProductRefId = "Product";

        public string GetBetTypeName(int betTypeId)
        {
            return RefData.GetBetTypeName(betTypeId.ToString(), DefaultLanguage);
        }

        public string GetSportName(int sportTypeId)
        {
            return RefData.GetSportName(sportTypeId.ToString(), DefaultLanguage);
        }

        public string GetProductName(int productId)
        {
            return RefData.GetResourceDataByRefIdAndResourceId(ProductRefId, productId.ToString(), DefaultLanguage, string.Empty);
        }
    }
}
