namespace Fanex.ProductStatus.Setting.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AppResources;
    using Models;
    using Services;
    using ViewModels;

    public class ProductStatusSettingController : Controller
    {
        private readonly ProductStatusService productStatusService;

        public ProductStatusSettingController()
            : this(null)
        {
        }

        public ProductStatusSettingController(ProductStatusService productStatusService)
        {
            this.productStatusService = productStatusService ?? new ProductStatusServiceImpl();
        }

        //[ProductStatusAuthorize(IsHost = false)]
        public ActionResult Index(QueryViewModel viewModel)
        {
            var productStatus = viewModel.IsValid()
                ? productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId)
                : Enumerable.Empty<ProductStatusModel>();

            viewModel.SetProductStatusViewModel(productStatus);

            return View(viewModel);
        }

        //[HttpPost, ProductStatusAuthorize(IsHost = true), ProductStatusValidateAntiForgeryToken]
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

       // [HttpPost, ProductStatusAuthorize(IsHost = true)]
        public ActionResult MultipleUpdate(MultipleUpdateViewModel viewModel)
        {
            var jsonRequestBehavior = JsonRequestBehavior.DenyGet;

            if (viewModel.IsValid())
            {
                var results = productStatusService.MultipleUpdateProductStatus(
                    viewModel.ProductId,
                    viewModel.Customers,
                    viewModel.ActorId,
                    viewModel.IP,
                    viewModel.ProductStatus);

                return Json(results, jsonRequestBehavior);
            }

            return Json(new Result(400, Resources.BadRequest), jsonRequestBehavior);
        }
    }
}