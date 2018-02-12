namespace Fanex.ProductStatus.Setting.Web
{
    using System.Web.Mvc;
    using AppResources;
    using Configuration;
    using Services;

    internal class ProductStatusAuthorizeAttribute : AuthorizeAttribute
    {
        public bool IsHost { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IProductStatusAuthorizeService authorizationService = ProductStatusConfigurationHandler.CreateAuthorizeService();

            bool isAuthorized = IsHost
                ? authorizationService.CanUpdate(filterContext)
                : authorizationService.CanView(filterContext);

            if (!isAuthorized)
            {
                UnAuthorizedAction(filterContext);
            }

            base.OnAuthorization(filterContext);
        }

        private void UnAuthorizedAction(AuthorizationContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.Write(Resources.UnauthorizeMessage);
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult(ProductStatusConfigurationHandler.GetUnAuthorizedUrl());
            }
        }
    }
}
