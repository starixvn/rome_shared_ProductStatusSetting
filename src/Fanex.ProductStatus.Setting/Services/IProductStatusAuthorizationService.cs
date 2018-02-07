using System.Web.Mvc;

namespace Fanex.ProductStatus.Setting.Services
{
    public interface IProductStatusAuthorizationService
    {
        bool CanView(AuthorizationContext authorizationContext);

        bool CanUpdate(AuthorizationContext authorizationContext);
    }
}
