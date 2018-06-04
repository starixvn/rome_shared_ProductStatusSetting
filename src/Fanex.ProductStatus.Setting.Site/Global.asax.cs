namespace Fanex.ProductStatus.Setting.Site
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Configuration;
    using Services;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ProductStatusConfigurationHandler.UseConfig(
                new ProductStatusConfiguration()
                .UseAuthorizeService(new ProductStatusAuthorizeService())
                .UseProductStatusActor(new ProductStatusActor())
                .SetUnAuthorizedUrl("~/Error/UnAuthorized"));

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
        }
    }

    public class ProductStatusAuthorizeService : IProductStatusAuthorizeService
    {
        public bool CanView(AuthorizationContext filterContext)
            => true;

        public bool CanUpdate(AuthorizationContext filterContext)
            => true;
    }

    public class ProductStatusActor : IProductStatusActor
    {
        public int Id { get; } = 8;

        public string Name { get; } = "sa";

        public string ClientIP { get; } = "127.0.0.1";

        // NAP = 1
        // Ext-NAP = 4
        public byte SystemId { get; } = 1;

        public IEnumerable<KeyValuePair<int, string>> GetProductList()
            => new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(3, "Number Game"),
                new KeyValuePair<int, string>(6, "Casino"),
                new KeyValuePair<int, string>(8, "Keno"),
                new KeyValuePair<int, string>(12, "Gold Deluxe"),
                new KeyValuePair<int, string>(15, "ColossusBets"),
                new KeyValuePair<int, string>(21, "Allbets"),
                new KeyValuePair<int, string>(22, "Macau Games"),
                new KeyValuePair<int, string>(23, "NG2"),
                new KeyValuePair<int, string>(24, "Fishing World"),
                new KeyValuePair<int, string>(30, "Bullfighting")
            };
    }
}