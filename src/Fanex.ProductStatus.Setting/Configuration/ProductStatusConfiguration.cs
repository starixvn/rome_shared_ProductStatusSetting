namespace Fanex.ProductStatus.Setting.Configuration
{
    using System;

    public class ProductStatusConfiguration
    {
        internal Type AuthorizeServiceType { get; private set; }

        internal Type UserServiceType { get; private set; }

        internal string UnAuthorizedUrl { get; private set; }

        public ProductStatusConfiguration UseAuthorizeService(Type authorizeType)
        {
            AuthorizeServiceType = authorizeType;
            return this;
        }

        public ProductStatusConfiguration UseUserService(Type userType)
        {
            UserServiceType = userType;
            return this;
        }

        public ProductStatusConfiguration SetUnAuthorizedUrl(string url)
        {
            UnAuthorizedUrl = url;
            return this;
        }
    }
}
