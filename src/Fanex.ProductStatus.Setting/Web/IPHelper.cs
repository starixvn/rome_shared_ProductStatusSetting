namespace Fanex.ProductStatus.Setting.Web
{
    using System;
    using System.Web;

    internal static class IPHelper
    {
        public static string GetClientIP()
        {
            var request = HttpContext.Current.Request;

            if (request.IsLocal)
            {
                return "127.0.0.1";
            }

            var serverVariables = request.ServerVariables;
            var ipAddress = serverVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                var addresses = ipAddress.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return serverVariables["REMOTE_ADDR"];
        }
    }
}
