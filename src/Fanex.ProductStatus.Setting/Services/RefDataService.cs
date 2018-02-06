using Fanex.ProductStatus.Setting.AppResources;
using Fanex.ProductStatus.Setting.Enumerations;
using RemotingRef;

namespace Fanex.ProductStatus.Setting.Services
{
    internal class RefDataService : IRefDataService
    {
        private const string DefaultLanguage = "en";
        private const string ProductRefId = "Product";

        public string GetBetTypeName(int betTypeId)
        {
            if (betTypeId == (int)BetTypeIdentity.AllbetStartRange)
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
