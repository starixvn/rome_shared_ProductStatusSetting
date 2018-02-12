namespace Fanex.ProductStatus.Setting.Controllers
{
    using System.Collections.Generic;
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

        // [ProductStatusAuthorization(IsHost = false)]
        public ActionResult Index(QueryViewModel viewModel)
        {
            var productStatuses = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    sportType: 5,
                    betType: 12,
                    isDisabled: true,
                    isDisabledUpline: false,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 78,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 37,
                    totalDisabledMembers: 36,
                    totalEnabledMembers: 46
                    ),
                new ProductStatusModel(
                    sportType: 5,
                    betType: 13,
                    isDisabled: true,
                    isDisabledUpline: false,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 78,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 37,
                    totalDisabledMembers: 36,
                    totalEnabledMembers: 46
                    ),
                  new ProductStatusModel(
                    sportType: 5,
                    betType: 14,
                    isDisabled: true,
                    isDisabledUpline: false,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 78,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 37,
                    totalDisabledMembers: 36,
                    totalEnabledMembers: 46
                    )
            };

            var fakeViewModel = new QueryViewModel
            {
                ProductId = 5,
                CustId = 123,
                CustName = "supertest1",
                CustRole = 4,
                ProductStatus = productStatuses
            };

            return View(fakeViewModel);

            //viewModel.ProductStatus = viewModel.IsValid()
            //    ? productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId)
            //    : Enumerable.Empty<ProductStatusModel>();

            //return View(viewModel);
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

        //[HttpPost, ProductStatusAuthorization(IsHost = true)]
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