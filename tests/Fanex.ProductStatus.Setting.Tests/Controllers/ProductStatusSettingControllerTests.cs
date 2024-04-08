namespace Fanex.ProductStatus.Setting.Tests.Controllers
{
    using Configuration;
    using Fanex.ProductStatus.Setting.Configuration;
    using NSubstitute;
    using Setting.Controllers;
    using Setting.Models;
    using Setting.Services;
    using Setting.Shared;
    using Setting.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Xunit;

    [Trait("Controllers", "ProductStatusSettingController")]
    public class ProductStatusSettingControllerTests
    {
        private readonly IProductStatusActor productStatusActor;
        private readonly ProductStatusService productStatusService;
        private readonly ProductStatusSettingController productStatusSettingController;

        public ProductStatusSettingControllerTests()
        {
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(new ProductStatusAuthorizeServiceForTesting())
                .UseProductStatusActor(new ProductStatusActorForTesting())
                .SetUnAuthorizedUrl("~/Error/UnAuthorized");

            ProductStatusConfigurationHandler.UseConfig(configuration);
            productStatusActor = Substitute.For<IProductStatusActor>();
            productStatusService = Substitute.For<ProductStatusService>();
            productStatusSettingController = new ProductStatusSettingController(productStatusService);
        }

        [Fact]
        public void Index_InvalidQueryViewModel_ReturnsEmptyData()
        {
            // Arrange
            var queryViewModel = new QueryViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Casino,
                CustRole = RoleIdentity.Super
            };
            productStatusService
                .GetProductStatus(queryViewModel.ProductId, queryViewModel.CustId, queryViewModel.SportTypeId, queryViewModel.SystemId, queryViewModel.SettingStatus)
                .Returns(Enumerable.Empty<ProductStatusModel>());

            // Assert
            Assert.False(queryViewModel.ProductStatus.Any());
        }

        [Fact]
        public void Index_ValidQueryViewModel_ReturnsView()
        {
            // Arrange
            var queryViewModel = new QueryViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Casino,
                CustId = 1176083,
                CustName = "supertest1",
                CustRole = RoleIdentity.Super
            };
            var productStatus = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    sportType: 208,
                    parentSportType: 0,
                    betType: 2101,
                    parentBetType: 0,
                    isDisabled: false,
                    isDisabledUpline: false,
                    totalDisabledMasters: 0,
                    totalEnabledMasters: 100,
                    totalDisabledAgents: 0,
                    totalEnabledAgents: 200,
                    totalDisabledMembers: 0,
                    totalEnabledMembers: 300)
            };
            productStatusService
                .GetProductStatus(queryViewModel.ProductId, queryViewModel.CustId, queryViewModel.SportTypeId, queryViewModel.SystemId, queryViewModel.SettingStatus)
                .Returns(productStatus);

            // Act
            var actualView = productStatusSettingController.Index(queryViewModel) as ViewResult;

            // Assert
            Assert.True(queryViewModel.ProductStatus.Any() && null != actualView);
        }

        [Fact]
        public void Update_InvalidUpdateViewModel_ReturnsBadRequest()
        {
            // Arrange
            productStatusActor.Id.Returns(8);
            var updateViewModel = new UpdateViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Casino
            };

            // Act
            var actualView = productStatusSettingController.Update(updateViewModel) as JsonResult;
            var actualResult = (Result)actualView.Data;

            // Assert
            Assert.Equal(400, actualResult.Code);
        }

        [Fact]
        public void Update_ValidUpdateViewModel_ReturnsSuccessfulJson()
        {
            // Arrange
            productStatusActor.Id.Returns(8);
            productStatusActor.Name.Returns("sa");
            productStatusActor.ClientIP.Returns("127.0.0.1");
            var updateViewModel = new UpdateViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Keno,
                ProductStatus = new[]
                {
                    new ProductStatusCommand
                    {
                        SportType = 208,
                        BetType = 2101,
                        Disable = false,
                        UpdateAllMasters = true,
                        DisableAllMasters = false,
                        UpdateAllAgents = true,
                        DisableAllAgents = false,
                        UpdateAllMembers = true,
                        DisableAllMembers = false,
                    }
                },
                CustId = 1176083,
                CustRole = 3
            };
            productStatusService.UpdateProductStatus(
                updateViewModel.IsGroup,
                updateViewModel.ProductId,
                updateViewModel.CustId,
                updateViewModel.CustRole,
                updateViewModel.ActorId,
                updateViewModel.ActorName,
                updateViewModel.IP,
                0,
                Arg.Any<IEnumerable<ProductStatusCommand>>(),
                updateViewModel.SystemId)
            .Returns(new List<Result> { new Result(0, string.Empty) });

            // Act
            var actualView = productStatusSettingController.Update(updateViewModel) as JsonResult;
            var actualResult = (IEnumerable<Result>)actualView.Data;

            // Assert
            Assert.True(actualResult.FirstOrDefault().IsSuccess());
        }

        [Fact]
        public void MultipleView_Always_ReturnsView()
        {
            // Arrange
            var viewModel = new MultipleQueryViewModel(
                Enumerable.Empty<Product>(),
                new ProductStatusActorForTesting());
            string expectedView = "~/Areas/ProductStatusSetting/Views/Multiple/Index.cshtml";

            // Act
            var actualView = productStatusSettingController.MultipleView(viewModel) as ViewResult;

            // Assert
            Assert.Equal(expectedView, actualView.ViewName);
        }

        [Fact]
        public void MultiplePartialView_InvalidQueryViewModel_ReturnsEmptyData()
        {
            // Arrange
            var queryViewModel = new QueryViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Casino,
                CustRole = RoleIdentity.Super
            };
            productStatusService
                .GetProductStatus(queryViewModel.ProductId, queryViewModel.CustId, queryViewModel.SportTypeId, queryViewModel.SystemId, queryViewModel.SettingStatus)
                .Returns(Enumerable.Empty<ProductStatusModel>());

            // Assert
            Assert.False(queryViewModel.ProductStatus.Any());
        }

        [Fact]
        public void MultiplePartialView_ValidQueryViewModel_ReturnsView()
        {
            // Arrange
            var queryViewModel = new QueryViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Casino,
                CustId = 1176083,
                CustRole = RoleIdentity.Super
            };
            var productStatus = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    sportType: 208,
                    parentSportType: 0,
                    betType: 2101,
                    parentBetType: 0,
                    isDisabled: false,
                    isDisabledUpline: false,
                    totalDisabledMasters: 0,
                    totalEnabledMasters: 100,
                    totalDisabledAgents: 0,
                    totalEnabledAgents: 200,
                    totalDisabledMembers: 0,
                    totalEnabledMembers: 300)
            };
            productStatusService
                .GetProductStatus(queryViewModel.ProductId, queryViewModel.CustId, queryViewModel.SportTypeId, queryViewModel.SystemId, queryViewModel.SettingStatus)
                .Returns(productStatus);

            // Act
            var actualView = productStatusSettingController.Index(queryViewModel) as ViewResult;

            // Assert
            Assert.True(queryViewModel.ProductStatus.Any() && null != actualView);
        }

        [Fact]
        public void MultipleUpdate_InvalidMultipleUpdateViewModel_ReturnsBadRequest()
        {
            // Arrange
            productStatusActor.Id.Returns(8);
            var updateViewModel = new MultipleUpdateViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Casino
            };

            // Act
            var actualView = productStatusSettingController.MultipleUpdate(updateViewModel) as JsonResult;
            var actualResult = (Result)actualView.Data;

            // Assert
            Assert.Equal(400, actualResult.Code);
        }

        [Fact]
        public void MultipleUpdate_ValidMultipleUpdateViewModel_ReturnsSuccessfulJson()
        {
            // Arrange
            productStatusActor.Id.Returns(8);
            productStatusActor.Name.Returns("sa");
            productStatusActor.ClientIP.Returns("127.0.0.1");
            var updateViewModel = new MultipleUpdateViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Casino,
                ProductStatus = new[]
                {
                    new ProductStatusCommand
                    {
                        SportType = 208,
                        BetType = 2101,
                        Disable = false,
                        UpdateAllMasters = true,
                        DisableAllMasters = false,
                        UpdateAllAgents = true,
                        DisableAllAgents = false,
                        UpdateAllMembers = true,
                        DisableAllMembers = false,
                    }
                },
                CustIds = "1176092,14244423",
                CustNames = "member01,member012",
                CustRole = RoleIdentity.Super
            };
            productStatusService.MultipleUpdateProductStatus(
                updateViewModel.IsGroup,
                updateViewModel.ProductId,
                updateViewModel.CustRole,
                updateViewModel.CustomerIds,
                updateViewModel.CustomerNames,
                updateViewModel.ActorId,
                updateViewModel.ActorName,
                updateViewModel.IP,
                0,
                updateViewModel.ProductStatus,
                updateViewModel.SystemId)
            .Returns(new[] { new Result(0, string.Empty) });

            // Act
            var actualView = productStatusSettingController.MultipleUpdate(updateViewModel) as JsonResult;
            var actualResults = (IList<Result>)actualView.Data;

            // Assert
            Assert.DoesNotContain(actualResults, result => !result.IsSuccess());
        }

        [Fact]
        public void Simple_InvalidSimpleQueryViewModel_ReturnsEmptyData()
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel(new ProductStatusActorForTesting())
            {
                ProductId = ProductIdentity.Casino,
                CustRole = RoleIdentity.Super
            };
            productStatusService
                .GetProductStatus(viewModel.ProductId, viewModel.CustId, viewModel.SportTypeId, viewModel.SystemId, viewModel.SettingStatus)
                .Returns(Enumerable.Empty<ProductStatusModel>());

            // Assert
            Assert.False(viewModel.ProductStatus.Any());
        }

        [Fact]
        public void Simple_ValidSimpleQueryViewModel_ReturnsView()
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel(new ProductStatusActorForTesting())
            {
                ProductId = ProductIdentity.Casino,
                CustId = 1176083,
                CustName = "supertest1",
                CustRole = RoleIdentity.Super
            };
            var productStatus = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    sportType: 208,
                    parentSportType: 0,
                    betType: 2101,
                    parentBetType: 0,
                    isDisabled: false,
                    isDisabledUpline: false,
                    totalDisabledMasters: 0,
                    totalEnabledMasters: 100,
                    totalDisabledAgents: 0,
                    totalEnabledAgents: 200,
                    totalDisabledMembers: 0,
                    totalEnabledMembers: 300)
            };
            productStatusService
                .GetProductStatus(viewModel.ProductId, viewModel.CustId, viewModel.SportTypeId, viewModel.SystemId, viewModel.SettingStatus)
                .Returns(productStatus);

            // Act
            var actualView = productStatusSettingController.Simple(viewModel) as ViewResult;

            // Assert
            Assert.True(viewModel.ProductStatus.Any() && null != actualView);
        }

        [Fact]
        public void SimpleUpdate_InvalidSimpleUpdateViewModel_ReturnsBadRequest()
        {
            // Arrange
            productStatusActor.Id.Returns(8);
            var viewModel = new SimpleUpdateViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Casino
            };

            // Act
            var actualView = productStatusSettingController.SimpleUpdate(viewModel) as JsonResult;
            var actualResult = (Result)actualView.Data;

            // Assert
            Assert.Equal(400, actualResult.Code);
        }

        [Fact]
        public void SimpleUpdate_ValidSimpleUpdateViewModel_ReturnsSuccessfulJson()
        {
            // Arrange
            productStatusActor.Id.Returns(8);
            productStatusActor.Name.Returns("sa");
            productStatusActor.ClientIP.Returns("127.0.0.1");
            var viewModel = new SimpleUpdateViewModel(productStatusActor)
            {
                ProductId = ProductIdentity.Keno,
                ProductStatus = new[]
                {
                    new SimpleProductStatusCommand
                    {
                        SportType = 208,
                        BetType = 2101,
                        Disable = false,
                        DisableDownlines = false
                    }
                },
                CustId = 1176083,
                CustRole = 3
            };
            productStatusService.UpdateProductStatus(
                viewModel.IsGroup,
                viewModel.ProductId,
                viewModel.CustId,
                viewModel.CustRole,
                viewModel.ActorId,
                viewModel.ActorName,
                viewModel.IP,
                0,
                Arg.Any<IEnumerable<ProductStatusCommand>>(),
                viewModel.SystemId)
            .Returns(new List<Result> { new Result(0, string.Empty) });

            // Act
            var actualView = productStatusSettingController.SimpleUpdate(viewModel) as JsonResult;
            var actualResult = (IEnumerable<Result>)actualView.Data;

            // Assert
            Assert.True(actualResult.FirstOrDefault().IsSuccess());
        }
    }
}