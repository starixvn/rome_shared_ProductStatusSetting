namespace Fanex.ProductStatus.Setting.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using NSubstitute;
    using Setting.Controllers;
    using Setting.Models;
    using Setting.Services;
    using Setting.ViewModels;
    using Shared;
    using Xunit;

    public class ProductStatusSettingControllerTests
    {
        private readonly IProductStatusUser user;
        private readonly ProductStatusService productStatusService;
        private readonly ProductStatusSettingController productStatusSettingController;

        public ProductStatusSettingControllerTests()
        {
            user = Substitute.For<IProductStatusUser>();
            productStatusService = Substitute.For<ProductStatusService>();
            productStatusSettingController = new ProductStatusSettingController(productStatusService);
        }

        [Fact]
        public void Index_InvalidQueryViewModel_ReturnsEmptyData()
        {
            // Arrange
            var queryViewModel = new QueryViewModel
            {
                ProductId = ProductIdentity.Casino,
                CustRole = RoleIdentity.Super
            };
            productStatusService
                .GetProductStatus(queryViewModel.ProductId, queryViewModel.CustId)
                .Returns(Enumerable.Empty<ProductStatusModel>());

            // Act
            var actualView = productStatusSettingController.Index(queryViewModel) as ViewResult;

            // Assert
            Assert.False(queryViewModel.ProductStatus.Any());
        }

        [Fact]
        public void Index_ValidQueryViewModel_ReturnsView()
        {
            // Arrange
            var queryViewModel = new QueryViewModel
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
                    betType: 2101,
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
                .GetProductStatus(queryViewModel.ProductId, queryViewModel.CustId)
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
            user.GetActorId().Returns(8);
            var updateViewModel = new UpdateViewModel(user)
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
            user.GetActorId().Returns(8);
            user.GetClientIP().Returns("127.0.0.1");
            var updateViewModel = new UpdateViewModel(user)
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
                CustId = 1176083
            };
            productStatusService
                .UpdateProductStatus(updateViewModel.ProductId, updateViewModel.CustId, updateViewModel.ActorId, updateViewModel.IP, updateViewModel.ProductStatus)
                .Returns(new Result(0, string.Empty));

            // Act
            var actualView = productStatusSettingController.Update(updateViewModel) as JsonResult;
            var actualResult = (Result)actualView.Data;

            // Assert
            Assert.True(actualResult.IsSuccess());
        }
    }
}
