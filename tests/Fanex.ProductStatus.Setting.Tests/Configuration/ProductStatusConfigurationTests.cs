namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using Setting.Configuration;
    using Xunit;

    public class ProductStatusConfigurationTests
    {
        [Fact]
        public void UseAuthorizeService_Always_AssignsCorrectToAuthorizeServiceType()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration();

            // Act
            configuration.UseAuthorizeService(typeof(ProductStatusAuthorizeServiceForTesting));

            // Assert
            Assert.NotNull(configuration.AuthorizeServiceType);
        }

        [Fact]
        public void UseUserService_Always_AssignsCorrectToUserServiceType()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration();

            // Act
            configuration.UseUserService(typeof(ProductStatusUserServiceForTesting));

            // Assert
            Assert.NotNull(configuration.UserServiceType);
        }

        [Fact]
        public void SetUnAuthorizedUrl_Always_AssignsCorrectToUnAuthorizedUrl()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration();

            // Act
            configuration.SetUnAuthorizedUrl("~/Error/UnAuthorized");

            // Assert
            Assert.NotNull(configuration.UnAuthorizedUrl);
        }
    }
}
