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
            configuration.UseAuthorizeService(new ProductStatusAuthorizeServiceForTesting());

            // Assert
            Assert.NotNull(configuration.AuthorizeService);
        }

        [Fact]
        public void UseUserSession_Always_AssignsCorrectToUser()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration();

            // Act
            configuration.UseUserSession(new ProductStatusUserServiceForTesting());

            // Assert
            Assert.NotNull(configuration.UserService);
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
