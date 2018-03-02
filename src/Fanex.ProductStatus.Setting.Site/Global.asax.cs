namespace Fanex.ProductStatus.Setting.Site
{
    using System;
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
                .UseUserSession(new ProductStatusUser())
                .SetUnAuthorizedUrl("~/Error/UnAuthorized"));

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
        }
    }

    public class ProductStatusAuthorizeService : IProductStatusAuthorizeService
    {
        public bool CanUpdate(AuthorizationContext filterContext)
            => true;

        public bool CanView(AuthorizationContext filterContext)
            => true;
    }

    public class ProductStatusUser : IProductStatusUser
    {
        public Func<int> GetActorId()
            => () => 8;

        public Func<string> GetClientIP()
            => () => "127.0.0.1";
    }
}