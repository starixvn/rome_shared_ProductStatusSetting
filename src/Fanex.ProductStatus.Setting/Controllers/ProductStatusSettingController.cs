namespace Fanex.ProductStatus.Setting.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AppResources;
    using Models;
    using Services;
    using ViewModels;
    using Web;

    public class ProductStatusSettingController : Controller
    {
        private const string IndexViewPath = "~/Areas/ProductStatusSetting/Views/Index.cshtml";
        private const string MultipleViewPath = "~/Areas/ProductStatusSetting/Views/MultipleView.cshtml";
        private const string MultiplePartialViewPath = "~/Areas/ProductStatusSetting/Views/MultiplePartialView.cshtml";

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
            var productStatus = viewModel.IsValid()
                ? productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId)
                : Enumerable.Empty<ProductStatusModel>();

            viewModel.SetProductStatusViewModel(productStatus);

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
            return View(MultipleViewPath, viewModel);
        }

        [ProductStatusAuthorize(IsHost = false)]
        public ActionResult MultipleView(int productId)
        {
            var productInfo = productStatusService.GetProductInfo(productId);

            return PartialView(MultiplePartialViewPath, productInfo);
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
                viewModel.CustIds,
                viewModel.CustNames,
                viewModel.ActorId,
                viewModel.IP,
                viewModel.ProductStatus);

            return Json(results, JsonRequestBehavior.DenyGet);
        }

        #endregion

        private ActionResult JsonBadRequest()
            => Json(new Result(400, Resources.BadRequest), JsonRequestBehavior.DenyGet);
    }
}