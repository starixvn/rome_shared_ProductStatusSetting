namespace Fanex.ProductStatus.Setting.Services
{
    using System.Threading;
    using AppResources;
    using RemotingRef;

    public interface RefDataService
    {
        string GetBetTypeName(int betTypeId);

        string GetProductName(int productId);

        string GetSportTypeName(int sportTypeId);
    }

    internal class RefDataServiceImpl : RefDataService
    {
        private const int FirstBetTypeIdOfAllbets = 2401;

        private readonly IRemotingRefService remotingRefService;

        public RefDataServiceImpl(IRemotingRefService remotingRefService = null)
        {
            this.remotingRefService = remotingRefService ?? new RemotingRefService();
        }

        public string GetBetTypeName(int betTypeId)
        {
            if (betTypeId == FirstBetTypeIdOfAllbets)
            {
                return Resources.BetType_2401;
            }

            return remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);
        }

         public string GetSportTypeName(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode);
            
        public string GetProductName(int productId)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId.ToString(),
                LanguageCode,
                string.Empty);

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}
