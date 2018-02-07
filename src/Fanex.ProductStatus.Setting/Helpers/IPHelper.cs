using System.Linq;
using System.Web;

namespace Fanex.ProductStatus.Setting.Helpers
{
    public static class IPHelper
    {
        public static string GetClientIP()
        {
            var serverVariables = HttpContext.Current.Request.ServerVariables;
            var ipAddress = serverVariables["HTTP_X_FORWARDED_FOR"] ?? serverVariables["REMOTE_ADDR"];

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                return ipAddress.Split(',').FirstOrDefault();
            }

            return ipAddress;
        }
    }
}
