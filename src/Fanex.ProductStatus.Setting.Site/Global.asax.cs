namespace Fanex.ProductStatus.Setting.Site
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Configuration;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //ProductStatusConfigurationHandler.UseConfig(
            //    new ProductStatusConfiguration()
            //    .UseAuthorizeService(new )
            //    .UseUserSession(new )
            //    .SetUnAuthorizedUrl("~/Error/UnAuthorized"));

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
        }
    }
}