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
            var jsonRequestBehavior = JsonRequestBehavior.DenyGet;

            if (viewModel.IsValid())
            {
                var result = productStatusService.UpdateProductStatus(
                    viewModel.ProductId,
                    viewModel.CustId,
                    viewModel.ActorId,
                    viewModel.IP,
                    viewModel.ProductStatus);

                return Json(result, jsonRequestBehavior);
            }

            return Json(new Result(400, Resources.BadRequest), jsonRequestBehavior);
        }
    }
}