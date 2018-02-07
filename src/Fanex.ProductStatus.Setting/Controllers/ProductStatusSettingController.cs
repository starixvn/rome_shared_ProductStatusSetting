using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Fanex.ProductStatus.Setting.Filters;
using Fanex.ProductStatus.Setting.Models;
using Fanex.ProductStatus.Setting.Services;
using Fanex.ProductStatus.Setting.ViewModels;

namespace Fanex.ProductStatus.Setting.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProductStatusSettingController : Controller
    {
        private readonly IProductStatusService productStatusService;

        public ProductStatusSettingController()
            : this(null)
        {
        }

        public ProductStatusSettingController(IProductStatusService productStatusService)
        {
            this.productStatusService = productStatusService ?? new ProductStatusService();
        }

        //[ProductStatusAuthorization]
        public ActionResult Index(ProductStatusViewModel viewModel)
        {
            viewModel.ProductStatuses = viewModel.IsInvalid()
                ? Enumerable.Empty<ProductStatusModel>()
                : productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId);

            return View(viewModel);
        }

        [HttpPost, ProductStatusAuthorization]
        public ActionResult Update()
        {
            return Json(new { }, JsonRequestBehavior.DenyGet);
        }
    }
}
