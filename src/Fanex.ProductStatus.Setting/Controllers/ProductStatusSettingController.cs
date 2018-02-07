using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Fanex.ProductStatus.Setting.Filters;
using Fanex.ProductStatus.Setting.Helpers;
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
        public ActionResult Index(QueryViewModel viewModel)
        {
            viewModel.ProductStatuses = viewModel.IsInvalid()
                ? Enumerable.Empty<ProductStatusModel>()
                : productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId);

            return View(viewModel);
        }

        [HttpPost, ProductStatusAuthorization(IsHost = true)]
        public ActionResult Update(
            int productId,
            int custId,
            int uplineId,
            UpdateViewModel[] productStatuses)
        {
            int userId = 0; // Todo: get from external service here ...
            var ip = IPHelper.GetClientIP();

            var updateProductStatusParams = productStatuses.Select(status => new UpdateProductStatusParam(
                custId,
                status.SportType,
                status.BetType,
                status.Disable,
                status.UpdateAllMasters,
                status.DisableAllMasters,
                status.UpdateAllAgents,
                status.DisableAllAgents,
                status.UpdateAllMembers,
                status.DisableAllMembers,
                uplineId,
                userId,
                ip));

            var result = productStatusService.UpdateProductStatus(productId, updateProductStatusParams);

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
