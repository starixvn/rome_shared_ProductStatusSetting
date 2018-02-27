namespace Fanex.ProductStatus.Setting.Services
{
    using AppResources;

    internal interface RefDataServiceProvider
    {
        string GetBetTypeName(int betTypeId);

        string GetProductName(int productId);
    }

    internal class RefDataServiceProviderImpl : RefDataServiceProvider
    {
        private const int FirstBetTypeIdOfAllbets = 2401;
        private const string DefaultLanguage = "en";
        private const string ProductRefId = "Product";

        private readonly RefDataService refDataService;

        public RefDataServiceProviderImpl(RefDataService refDataService = null)
        {
            this.refDataService = refDataService ?? new RefDataServiceImpl();
        }

        public string GetBetTypeName(int betTypeId)
        {
            if (betTypeId == FirstBetTypeIdOfAllbets)
            {
                return Resources.BetType_2401;
            }

            return refDataService.GetBetTypeName(betTypeId.ToString(), DefaultLanguage);
        }

        public string GetProductName(int productId)
            => refDataService.GetResourceDataByRefIdAndResourceId(ProductRefId, productId.ToString(), DefaultLanguage);
    }
}
