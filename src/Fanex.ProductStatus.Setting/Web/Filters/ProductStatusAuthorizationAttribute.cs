using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Fanex.ProductStatus.Setting.AppResources;
using Fanex.ProductStatus.Setting.Configuration;
using Fanex.ProductStatus.Setting.Services;

namespace Fanex.ProductStatus.Setting.Web.Filters
{
    public class ProductStatusAuthorizationAttribute : AuthorizeAttribute
    {
        private const string AntiForgeryFormTokenName = "__RequestVerificationToken";
        private IProductStatusAuthorizationService authorizationService;

        public bool IsHost { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool isAuthorized = false;
            base.OnAuthorization(filterContext);

            if (!string.IsNullOrEmpty(ProductStatusSettings.Settings.AuthorizationServiceType))
            {
                var authorizationType = Type.GetType(ProductStatusSettings.Settings.AuthorizationServiceType);
                authorizationService = Activator.CreateInstance(authorizationType) as IProductStatusAuthorizationService;

                if (authorizationService != null)
                {
                    isAuthorized = IsHost
                        ? authorizationService.CanUpdate(filterContext)
                        : authorizationService.CanView(filterContext);

                    if (IsHost && isAuthorized)
                    {
                        isAuthorized = ValidateAntiXSRF();
                    }
                }
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
                filterContext.Result = new RedirectResult(ProductStatusSettings.Settings.UnAuthorizedPath);
            }
        }
    }
}
