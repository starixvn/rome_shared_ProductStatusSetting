namespace Fanex.ProductStatus.Setting.Web
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using AppResources;
    using Configuration;
    using Models;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal class ProductStatusAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public bool IsHost { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // TODO: Implement authorization logic for ASP.NET Core
            // The old IProductStatusAuthorizeService interface needs to be updated
            // to work with AuthorizationFilterContext instead of AuthorizationContext
            
            /*
            var authorizationService = ProductStatusConfigurationHandler.AuthorizeService;

            var isAuthorized = IsHost
                ? authorizationService.CanUpdate(context)
                : authorizationService.CanView(context);

            if (!isAuthorized)
            {
                UnAuthorizedAction(context);
            }
            */
            
            // Temporary: Allow all requests
            // throw new NotImplementedException("Authorization logic needs to be migrated to ASP.NET Core");
        }

        private static void UnAuthorizedAction(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            
            // Check if AJAX request (simplified check)
            bool isAjax = request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                context.Result = new JsonResult(new[] { new Result((int)HttpStatusCode.Forbidden, Resources.UnauthorizeMessage) });
            }
            else
            {
                context.Result = new RedirectResult(ProductStatusConfigurationHandler.UnAuthorizedUrl);
            }
        }
    }
}
