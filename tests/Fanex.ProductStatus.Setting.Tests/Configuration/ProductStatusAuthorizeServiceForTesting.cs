namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using System;
    using System.Web.Mvc;
    using Setting.Services;

    public class ProductStatusAuthorizeServiceForTesting : IProductStatusAuthorizeService
    {
        public bool CanView(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }

        public bool CanUpdate(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }

        public bool CanViewMultiple(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }

        public bool CanUpdateMultiple(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}
