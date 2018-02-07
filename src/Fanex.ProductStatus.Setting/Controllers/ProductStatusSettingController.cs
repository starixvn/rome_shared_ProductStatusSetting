using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Fanex.ProductStatus.Setting.Models;
using Fanex.ProductStatus.Setting.Services;
using Fanex.ProductStatus.Setting.ViewModels;
using Fanex.ProductStatus.Setting.Web.Filters;
using Fanex.ProductStatus.Setting.Web.Helpers;

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

        [ProductStatusAuthorization(IsHost = false)]
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
            var updateProductStatusParams = ToUpdateProductStatusParams(custId, uplineId, productStatuses);

            var result = productStatusService.UpdateProductStatus(productId, updateProductStatusParams);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        private static IEnumerable<UpdateProductStatusParam> ToUpdateProductStatusParams(
            int custId,
            int uplineId,
            UpdateViewModel[] productStatuses)
        {
            int userId = 0; // Todo: get from external service here ...
            var ip = IPHelper.GetClientIP();

            return productStatuses.Select(status => new UpdateProductStatusParam(
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
        }
    }
}
