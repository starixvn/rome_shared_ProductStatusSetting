namespace Fanex.ProductStatus.Setting.Services
{
    using System;
    using System.Threading;
    // TODO: Requires Fanex.RemotingRef migration
    // using RemotingRef;

    public interface RefDataService
    {
        string GetBetTypeName(int betTypeId);

        string GetProductName(int productId);

        string GetSportTypeName(int sportTypeId);
    }

    internal class RefDataServiceImpl : RefDataService
    {
        // TODO: Requires Fanex.RemotingRef migration
        // private readonly IRemotingRefService remotingRefService;

        // public RefDataServiceImpl(IRemotingRefService remotingRefService = null)
        // {
        //     this.remotingRefService = remotingRefService ?? new RemotingRefService();
        // }

        public string GetBetTypeName(int betTypeId)
        {
            // TODO: Requires Fanex.RemotingRef migration
            throw new NotImplementedException("Requires Fanex.RemotingRef migration");
            // => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);
        }

        public string GetSportTypeName(int sportTypeId)
        {
            // TODO: Requires Fanex.RemotingRef migration
            throw new NotImplementedException("Requires Fanex.RemotingRef migration");
            // => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode, null) ?? remotingRefService.GetResourceDataByRefIdAndResourceId(
            //     "DummySport",
            //     sportTypeId.ToString(),
            //     LanguageCode,
            //     string.Empty);
        }

        public string GetProductName(int productId)
        {
            // TODO: Requires Fanex.RemotingRef migration
            throw new NotImplementedException("Requires Fanex.RemotingRef migration");
            // => remotingRefService.GetResourceDataByRefIdAndResourceId(
            //     "Product",
            //     productId.ToString(),
            //     LanguageCode,
            //     string.Empty);
        }

        // private static string LanguageCode
        //     => Thread.CurrentThread.CurrentCulture.Name;
    }
}
