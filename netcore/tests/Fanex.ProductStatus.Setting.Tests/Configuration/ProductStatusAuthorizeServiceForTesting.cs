namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Setting.Services;

    public class ProductStatusAuthorizeServiceForTesting : IProductStatusAuthorizeService
    {
        public bool CanView(AuthorizationFilterContext filterContext)
        {
            throw new NotImplementedException();
        }

        public bool CanUpdate(AuthorizationFilterContext filterContext)
        {
            throw new NotImplementedException();
        }

        public bool CanViewMultiple(AuthorizationFilterContext filterContext)
        {
            throw new NotImplementedException();
        }

        public bool CanUpdateMultiple(AuthorizationFilterContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}
