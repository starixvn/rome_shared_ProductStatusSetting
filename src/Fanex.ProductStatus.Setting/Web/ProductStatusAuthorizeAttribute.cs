namespace Fanex.ProductStatus.Setting.Web
{
    using System;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using AppResources;
    using Configuration;
    using Services;

    public class ProductStatusAuthorizeAttribute : AuthorizeAttribute
    {
        private const string AntiForgeryFormTokenName = "__RequestVerificationToken";

        public bool IsHost { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            IProductStatusAuthorizeService authorizationService = ProductStatusConfigurationHandler.CreateAuthorizeService();

            bool isAuthorized = IsHost
                ? authorizationService.CanUpdate(filterContext)
                : authorizationService.CanView(filterContext);

            if (IsHost && isAuthorized)
            {
                isAuthorized = ValidateAntiXSRF();
            }

            if (!isAuthorized)
            {
                UnAuthorizedAction(filterContext);
            }
        }

        private bool ValidateAntiXSRF()
        {
            try
            {
                var request = HttpContext.Current.Request;
                var formToken = request.Headers[AntiForgeryFormTokenName];

                if (string.IsNullOrWhiteSpace(formToken))
                {
                    return false;
                }

                var cookieValue = request.Cookies[AntiForgeryConfig.CookieName];

                if (cookieValue != null && !string.IsNullOrWhiteSpace(cookieValue.Value))
                {
                    AntiForgery.Validate(cookieValue.Value, formToken);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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
