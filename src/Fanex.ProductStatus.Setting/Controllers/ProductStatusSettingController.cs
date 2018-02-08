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
        }

        //[HttpPost, ProductStatusAuthorization(IsHost = true)]
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
