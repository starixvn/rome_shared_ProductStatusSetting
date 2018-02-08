namespace Fanex.ProductStatus.Setting.Configuration
{
    using System;

    public class ProductStatusSettings
    {
        internal Type AuthorizeServiceType { get; private set; }

        internal Type UserServiceType { get; private set; }

        internal string UnAuthorizedUrl { get; private set; }

        public ProductStatusSettings UseAuthorizeService(Type authorizeType)
        {
            AuthorizeServiceType = authorizeType;
            return this;
        }

        public ProductStatusSettings UseUserService(Type userType)
        {
            UserServiceType = userType;
            return this;
        }

        public ProductStatusSettings SetUnAuthorizedUrl(string url)
        {
            UnAuthorizedUrl = url;
            return this;
        }
    }
}
