namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using Fanex.ProductStatus.Setting.Configuration;
    using Fanex.ProductStatus.Setting.Criteria;
    using Fanex.ProductStatus.Setting.DataAccess;
    using Fanex.ProductStatus.Setting.Services;
    using Fanex.ProductStatus.Setting.Tests.Configuration;
    using NSubstitute;
    using Setting.Models;
    using Xunit;

    [Trait("Services", "ProductProfileServiceImpl")]
    public class ProductProfileServiceTests
    {
        private readonly ProductProfileDataAccess productProfileDataAccess;
        private readonly ProductProfileService productProfileService;
        private readonly IProductStatusActor productStatusActor;

        public ProductProfileServiceTests()
        {
            //// Arrange
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(new ProductStatusAuthorizeServiceForTesting())
                .UseProductStatusActor(new ProductStatusActorForTesting())
                .SetUnAuthorizedUrl("~/Error/UnAuthorized");

            ProductStatusConfigurationHandler.UseConfig(configuration);
            productProfileDataAccess = Substitute.For<ProductProfileDataAccess>();
            productStatusActor = Substitute.For<IProductStatusActor>();
            productProfileService = new ProductProfileService(productProfileDataAccess, productStatusActor);
        }

        [Theory]
        [InlineData(8, 3)]
        [InlineData(3, 1)]
        public void GetSettingLevel_NonDCS_ReturnValueFromHardcode(int productId, int exceptedValue)
        {
            // Act
            var act = productProfileService.GetSettingLevel(productId, 4);

            // Assert
            Assert.Equal(exceptedValue, act);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(16)]
        [InlineData(41)]
        [InlineData(23)]
        [InlineData(24)]
        [InlineData(44)]
        [InlineData(42)]
        public void GetSettingLevel_DCSProduct_CallRepository(int productId)
        {
            productProfileDataAccess
                .GetProductProfile(Arg.Any<SelectProductProfileCriteria>())
                .Returns(new ProductProfile(productId, 1, 1));

            //// Act
            var act = productProfileService.GetSettingLevel(productId, 4);

            //// Assert
            productProfileDataAccess.Received().GetProductProfile(Arg.Any<SelectProductProfileCriteria>());
        }
    }
}
