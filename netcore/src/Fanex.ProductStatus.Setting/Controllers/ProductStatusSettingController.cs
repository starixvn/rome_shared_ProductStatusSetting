namespace Fanex.ProductStatus.Setting.Controllers
{
    using System.Linq;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using AppResources;
    using Configuration;
    using Models;
    using Services;
    using ViewModels;
    using Web;

    public class ProductStatusSettingController : Controller
    {
        private const string SingleViewPath = "~/Areas/ProductStatusSetting/Views/Single/Index.cshtml";
        private const string MultipleViewPath = "~/Areas/ProductStatusSetting/Views/Multiple/Index.cshtml";
        private const string SimpleViewPath = "~/Areas/ProductStatusSetting/Views/Simple/Index.cshtml";

        private readonly ProductStatusService productStatusService;

        public ProductStatusSettingController(ProductStatusService? productStatusService = null)
        {
            this.productStatusService = productStatusService ?? new ProductStatusServiceImpl();
        }

        [ProductStatusAuthorize(IsHost = false)]
        public IActionResult Index(QueryViewModel viewModel)
        {
            PopulateQueryViewModel(viewModel);

            return View(SingleViewPath, viewModel);
        }

        [HttpPost, ProductStatusAuthorize(IsHost = true), ProductStatusValidateAntiForgeryToken]
        public IActionResult Update(UpdateViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return JsonBadRequest();
            }

            var result = productStatusService.UpdateProductStatus(
                    viewModel.IsGroup,
                    viewModel.ProductId,
                    viewModel.CustId,
                    viewModel.CustRole,
                    viewModel.ActorId,
                    viewModel.ActorName,
                    viewModel.IP,
                    viewModel.SettingStatus,
                    viewModel.ProductStatus.Select(product => product.CorrectProperties(viewModel.CustRole)).ToList(),
                    viewModel.SystemId);

            return Json(result);
        }

        #region Multiple Update

        [HttpPost]
        public IActionResult MultipleView(MultipleQueryViewModel viewModel)
        {
            if (viewModel.ProductId != 0 && CanView())
            {
                PopulateQueryViewModel(viewModel);
            }

            return View(MultipleViewPath, viewModel);
        }

        [HttpPost, ProductStatusAuthorize(IsHost = true), ProductStatusValidateAntiForgeryToken]
        public IActionResult MultipleUpdate(MultipleUpdateViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return JsonBadRequest();
            }

            var results = productStatusService.MultipleUpdateProductStatus(
                viewModel.IsGroup,
                viewModel.ProductId,
                viewModel.CustRole,
                viewModel.CustomerIds,
                viewModel.CustomerNames,
                viewModel.ActorId,
                viewModel.ActorName,
                viewModel.IP,
                viewModel.SettingStatus,
                viewModel.ProductStatus,
                viewModel.SystemId);

            return Json(results);
        }

        #endregion Multiple Update

        #region Old Behavior

        [ProductStatusAuthorize(IsHost = false)]
        public IActionResult Simple(SimpleQueryViewModel viewModel)
        {
            if (viewModel.IsValid())
            {
                viewModel.SetSimpleProductStatusViewModel(
                    productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId, viewModel.SportTypeId, viewModel.SystemId, ProductStatusConfigurationHandler.Actor.SettingStatus));
            }

            return View(SimpleViewPath, viewModel);
        }

        [HttpPost, ProductStatusAuthorize(IsHost = true), ProductStatusValidateAntiForgeryToken]
        public IActionResult SimpleUpdate(SimpleUpdateViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return JsonBadRequest();
            }

            var result = productStatusService.UpdateProductStatus(
                viewModel.IsGroup,
                viewModel.ProductId,
                viewModel.CustId,
                viewModel.CustRole,
                viewModel.ActorId,
                viewModel.ActorName,
                viewModel.IP,
                viewModel.SettingStatus,
                viewModel.ProductStatus.Select(product => product.ToProductStatusCommand(viewModel.CustRole)).ToList(),
                viewModel.SystemId);

            return Json(result);
        }

        #endregion Old Behavior

        private void PopulateQueryViewModel(BaseQueryViewModel viewModel)
        {
            if (viewModel.IsValid())
            {
                viewModel.SetProductStatusViewModel(
                    viewModel.IsGroup
                    ? productStatusService.GetProductStatusByGroup(viewModel.ProductId, viewModel.CustId, viewModel.SystemId, ProductStatusConfigurationHandler.Actor.SettingStatus)
                    : productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId, viewModel.SportTypeId, viewModel.SystemId, ProductStatusConfigurationHandler.Actor.SettingStatus));
            }
        }

        private IActionResult JsonBadRequest()
            => Json(new Result((int)HttpStatusCode.BadRequest, Resources.BadRequest));

        private bool CanView()
        {
            // TODO: Implement CanView logic for ASP.NET Core
            // The authorization context creation needs to be updated for ASP.NET Core
            
            /*
            var authorizationService = ProductStatusConfigurationHandler.AuthorizeService;
            return authorizationService.CanView(filterContext);
            */
            
            // Temporary: Allow all requests
            return true;
            // throw new NotImplementedException("CanView logic needs to be migrated to ASP.NET Core");
        }
    }
}