namespace Alpha.MemberInfo.Site
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();

            //ProductStatusSettings settings = new ProductStatusSettings()
            //    .UseAuthorizeService(typeof(A))
            //    .UseUserService(typeof(B))
            //    .SetUnAuthorizedUrl("/a/b/c");

            //ProductStatusSettingsHandler.UseSettings(settings);
            //ProductStatusSettingsHandler.Verify();
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError().GetBaseException();
            var isIgnoreError = Request.IsLocal ||
                ex is HttpException ||
                ex.Message.Contains("Extend Token Error UserID");

            if (!isIgnoreError)
            {
            }
        }

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }
    }
}