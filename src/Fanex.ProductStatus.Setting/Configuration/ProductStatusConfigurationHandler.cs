namespace Fanex.ProductStatus.Setting.Configuration
{
    using System;
    using Services;

    public static class ProductStatusConfigurationHandler
    {
        private static ProductStatusConfiguration statusSettings;

        public static void UseConfig(ProductStatusConfiguration settings)
        {
            statusSettings = settings;

            Verify();
        }

        internal static IProductStatusAuthorizeService AuthorizeService
            => statusSettings.AuthorizeService;

        internal static IProductStatusUserService UserService
            => statusSettings.UserService;

        internal static string UnAuthorizedUrl
            => statusSettings.UnAuthorizedUrl;

        private static void Verify()
        {
            RequireProductStatusSettings();

            RequireAuthorizeService();

            RequireUser();

            RequireUnAuthorizedUrl();
        }

        private static void RequireProductStatusSettings()
        {
            if (statusSettings == null)
            {
                throw new ArgumentNullException(
                    $"ProductStatus {nameof(statusSettings)}",
                    "Value cannot be null.");
            }
        }

        private static void RequireAuthorizeService()
        {
            if (statusSettings.AuthorizeService == null)
            {
                throw new ArgumentNullException(
                    $"ProductStatus {nameof(statusSettings.AuthorizeService)}",
                    "Value cannot be null.");
            }
        }

        private static void RequireUser()
        {
            if (statusSettings.UserService == null)
            {
                throw new ArgumentNullException(
                    $"ProductStatus {nameof(statusSettings.UserService)}",
                    "Value cannot be null.");
            }
        }

        private static void RequireUnAuthorizedUrl()
        {
            if (string.IsNullOrWhiteSpace(statusSettings.UnAuthorizedUrl))
            {
                throw new ArgumentNullException(
                    $"ProductStatus {nameof(statusSettings.UnAuthorizedUrl)}",
                    "Value cannot be null or empty.");
            }
        }
    }
}
