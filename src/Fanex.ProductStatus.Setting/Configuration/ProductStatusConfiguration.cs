namespace Fanex.ProductStatus.Setting.Configuration
{
    using Services;

    public class ProductStatusConfiguration
    {
        internal IProductStatusAuthorizeService AuthorizeService { get; private set; }

        internal IProductStatusUserService UserService { get; private set; }

        internal string UnAuthorizedUrl { get; private set; }

        public ProductStatusConfiguration UseAuthorizeService(IProductStatusAuthorizeService authorizeService)
        {
            AuthorizeService = authorizeService;
            return this;
        }

        public ProductStatusConfiguration UseUserSession(IProductStatusUserService userService)
        {
            UserService = userService;
            return this;
        }

        public ProductStatusConfiguration SetUnAuthorizedUrl(string url)
        {
            UnAuthorizedUrl = url;
            return this;
        }
    }
}
