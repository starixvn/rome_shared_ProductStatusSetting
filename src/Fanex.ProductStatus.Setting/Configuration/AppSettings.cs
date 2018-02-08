using System.Configuration;

namespace Fanex.ProductStatus.Setting.Configuration
{
    public class AppSettings : ConfigurationSection
    {
        private const string ProductStatusSettingsKey = "ProductStatusSettings";
        private const string UnAuthorizedPathKey = "UnAuthorizedPath";
        private const string AuthorizationServiceTypeKey = "AuthorizationServiceType";

        private static AppSettings _settings =
            ConfigurationManager.GetSection(ProductStatusSettingsKey) as AppSettings;

        public static AppSettings Settings => _settings;

        [ConfigurationProperty(UnAuthorizedPathKey, IsRequired = true)]
        public string UnAuthorizedPath
        {
            get
            {
                return (string)this[UnAuthorizedPathKey];
            }

            set
            {
                this[UnAuthorizedPathKey] = value;
            }
        }

        [ConfigurationProperty(AuthorizationServiceTypeKey, IsRequired = true)]
        public string AuthorizationServiceType
        {
            get
            {
                return (string)this[AuthorizationServiceTypeKey];
            }

            set
            {
                this[AuthorizationServiceTypeKey] = value;
            }
        }
    }
}
