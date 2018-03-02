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
        private readonly ProductStatusService productStatusService;

        public ProductStatusSettingController()
            : this(null)
        {
        }

        public ProductStatusSettingController(ProductStatusService productStatusService)
        {
            this.productStatusService = productStatusService ?? new ProductStatusServiceImpl();
        }

       // [ProductStatusAuthorize(IsHost = false)]
        public ActionResult Index(QueryViewModel viewModel)
        {
            var productStatus = viewModel.IsValid()
                ? productStatusService.GetProductStatus(viewModel.ProductId, viewModel.CustId)
                : Enumerable.Empty<ProductStatusModel>();

            viewModel.SetProductStatusViewModel(productStatus);

            return View(viewModel);

            //var productStatus = new List<ProductStatusModel>
            //{
            //    new ProductStatusModel(
            //        sportType: 5,
            //        betType: 12,
            //        isDisabled: true,
            //        isDisabledUpline: false,
            //        totalDisabledMasters: 12,
            //        totalEnabledMasters: 78,
            //        totalDisabledAgents: 33,
            //        totalEnabledAgents: 37,
            //        totalDisabledMembers: 36,
            //        totalEnabledMembers: 46),
            //    new ProductStatusModel(
            //        sportType: 5,
            //        betType: 13,
            //        isDisabled: false,
            //        isDisabledUpline: false,
            //        totalDisabledMasters: 12,
            //        totalEnabledMasters: 0,
            //        totalDisabledAgents: 33,
            //        totalEnabledAgents: 0,
            //        totalDisabledMembers: 36,
            //        totalEnabledMembers: 0),
            //      new ProductStatusModel(
            //        sportType: 5,
            //        betType: 14,
            //        isDisabled: false,
            //        isDisabledUpline: false,
            //        totalDisabledMasters: 12,
            //        totalEnabledMasters: 78,
            //        totalDisabledAgents: 33,
            //        totalEnabledAgents: 37,
            //        totalDisabledMembers: 0,
            //        totalEnabledMembers: 0)
            //};

            //var fakeViewModel = new QueryViewModel
            //{
            //    ProductId = 5,
            //    CustId = 123,
            //    CustName = "supertest1",
            //    CustRole = 4,
            //};

            //fakeViewModel.SetProductStatusViewModel(productStatus);

            //return View(fakeViewModel);
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
                    IPHelper.GetClientIP(),
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
                    IPHelper.GetClientIP(),
                    viewModel.ProductStatus);

                return Json(results, jsonRequestBehavior);
            }

            return Json(new Result(400, Resources.BadRequest), jsonRequestBehavior);
        }
    }
}