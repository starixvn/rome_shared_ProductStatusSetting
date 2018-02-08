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

        internal static IProductStatusAuthorizeService CreateAuthorizeService()
        {
            var authorizeService = Activator.CreateInstance(statusSettings.AuthorizeServiceType) as IProductStatusAuthorizeService;

            if (authorizeService == null)
            {
                throw new InvalidCastException("Cannot cast to IProductStatusAuthorizeService");
            }

            return authorizeService;
        }

        internal static IProductStatusUserService CreateUserService()
        {
            var userService = Activator.CreateInstance(statusSettings.UserServiceType) as IProductStatusUserService;

            if (userService == null)
            {
                throw new InvalidCastException("Cannot cast to IProductStatusUserService.");
            }

            return userService;
        }

        internal static string GetUnAuthorizedUrl()
        {
            return statusSettings.UnAuthorizedUrl;
        }

        private static void Verify()
        {
            RequireProductStatusSettings();

            RequireAuthorizeServiceType();

            RequireUserServiceType();

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

        private static void RequireAuthorizeServiceType()
        {
            if (statusSettings.AuthorizeServiceType == null)
            {
                throw new ArgumentNullException(
                    $"ProductStatus {nameof(statusSettings.AuthorizeServiceType)}",
                    "Value cannot be null.");
            }
        }

        private static void RequireUserServiceType()
        {
            if (statusSettings.UserServiceType == null)
            {
                throw new ArgumentNullException(
                    $"ProductStatus {nameof(statusSettings.UserServiceType)}",
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
