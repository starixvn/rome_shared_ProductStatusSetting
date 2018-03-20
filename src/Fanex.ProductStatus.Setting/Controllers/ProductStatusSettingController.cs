namespace Fanex.ProductStatus.Setting.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using AppResources;
    using Configuration;
    using Models;
    using Services;
    using ViewModels;
    using Web;

    public class ProductStatusSettingController : Controller
    {
        private const string IndexViewPath = "~/Areas/ProductStatusSetting/Views/Index.cshtml";
        private const string MultipleViewPath = "~/Areas/ProductStatusSetting/Views/MultipleView.cshtml";

        private readonly ProductStatusService productStatusService;

        public ProductStatusSettingController()
            : this(null)
        {
        }

        public ProductStatusSettingController(ProductStatusService productStatusService)
        {
            this.productStatusService = productStatusService ?? new ProductStatusServiceImpl();
        }

        [ProductStatusAuthorize(IsHost = false)]
        public ActionResult Index(QueryViewModel viewModel)
        {
            PopulateQueryViewModel(viewModel);

            return View(IndexViewPath, viewModel);
        }

        [HttpPost, ProductStatusAuthorize(IsHost = true), ProductStatusValidateAntiForgeryToken]
        public ActionResult Update(UpdateViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return JsonBadRequest();
            }

            var result = productStatusService.UpdateProductStatus(
                viewModel.ProductId,
                viewModel.CustId,
                viewModel.ActorId,
                viewModel.IP,
                viewModel.ProductStatus);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        #region Multiple Update

        [HttpPost]
        public ActionResult MultipleView(MultipleQueryViewModel viewModel)
        {
            if (viewModel.ProductId != 0 && CanView())
            {
                PopulateQueryViewModel(viewModel);
            }

            return View(MultipleViewPath, viewModel);
        }

        [HttpPost, ProductStatusAuthorize(IsHost = true), ProductStatusValidateAntiForgeryToken]
        public ActionResult MultipleUpdate(MultipleUpdateViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return JsonBadRequest();
            }

            var results = productStatusService.MultipleUpdateProductStatus(
                viewModel.ProductId,
                viewModel.CustomerIds,
                viewModel.CustomerNames,
                viewModel.ActorId,
                viewModel.IP,
                viewModel.ProductStatus);

            return Json(results, JsonRequestBehavior.DenyGet);
        }

        #endregion

        private void PopulateQueryViewModel(BaseQueryViewModel viewModel)
        {
            if (viewModel.IsValid())
            {
                viewModel.SetProductStatusViewModel(
                    productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId));
            }
        }

        private ActionResult JsonBadRequest()
            => Json(new Result((int)HttpStatusCode.BadRequest, Resources.BadRequest), JsonRequestBehavior.DenyGet);

        private bool CanView()
        {
            var filterContext = new AuthorizationContext(ControllerContext, null);
            var authorizationService = ProductStatusConfigurationHandler.AuthorizeService;

            return authorizationService.CanView(filterContext);
        }
    }
}