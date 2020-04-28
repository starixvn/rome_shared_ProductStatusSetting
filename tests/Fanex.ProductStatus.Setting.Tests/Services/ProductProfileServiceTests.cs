namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using Fanex.ProductStatus.Setting.Criteria;
    using Fanex.ProductStatus.Setting.DataAccess;
    using Fanex.ProductStatus.Setting.Services;
    using NSubstitute;
    using Setting.Models;
    using Xunit;

    [Trait("Services", "ProductProfileServiceImpl")]
    public class ProductProfileServiceTests
    {
        private readonly ProductProfileDataAccess productProfileDataAccess;
        private readonly ProductProfileService productProfileService;

        public ProductProfileServiceTests()
        {
            productProfileDataAccess = Substitute.For<ProductProfileDataAccess>();
            productProfileService = new ProductProfileService(productProfileDataAccess);
        }

        [Theory]
        [InlineData(6, 1)]
        [InlineData(8, 3)]
        [InlineData(3, 1)]
        [InlineData(12, 1)]
        public void GetSettingLevel_NonDCS_ReturnValueFromHardcode(int productId, int exceptedValue)
        {
            // Arrange

            // Act
            var act = productProfileService.GetSettingLevel(productId, 4);

            // Assert
            Assert.Equal(exceptedValue, act);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(16)]
        [InlineData(41)]
        [InlineData(22)]
        [InlineData(23)]
        [InlineData(24)]
        [InlineData(44)]
        [InlineData(42)]
        public void GetSettingLevel_DCSProduct_CallRepository(int productId)
        {
            //// Arrange
            productProfileDataAccess
                .GetProductProfile(Arg.Any<SelectProductProfileCriteria>())
                .Returns(new ProductProfile(33, 1, 1));

            //// Act
            var act = productProfileService.GetSettingLevel(productId, 4);

            //// Assert
            productProfileDataAccess.Received().GetProductProfile(Arg.Any<SelectProductProfileCriteria>());
        }
    }
}
