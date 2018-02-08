namespace Fanex.ProductStatus.Setting.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using AppResources;
    using Models;
    using Services;
    using ViewModels;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
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

        [ProductStatusAuthorize(IsHost = false)]
        public ActionResult Index(QueryViewModel viewModel)
        {
            viewModel.ProductStatus = viewModel.IsValid()
                ? productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId)
                : Enumerable.Empty<ProductStatusModel>();

            return View(viewModel);
        }

        [HttpPost, ProductStatusAuthorize(IsHost = true), ValidateAntiForgeryToken]
        public ActionResult Update(UpdateViewModel viewModel)
        {
            Result result;

            if (viewModel.IsValid())
            {
                viewModel = UpdateProductStatusCommands(viewModel);

                result = productStatusService.UpdateProductStatus(
                    viewModel.ProductId,
                    viewModel.ProductStatus);
            }
            else
            {
                result = new Result(400, Resources.BadRequest);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        private static UpdateViewModel UpdateProductStatusCommands(UpdateViewModel viewModel)
        {
            foreach (var item in viewModel.ProductStatus)
            {
                item.CustId = viewModel.CustId;
                item.ActorId = viewModel.ActorId;
                item.IP = viewModel.IP;
            }

            return viewModel;
        }
    }
}
