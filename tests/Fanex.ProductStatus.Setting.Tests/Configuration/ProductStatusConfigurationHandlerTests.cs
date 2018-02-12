namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using System;
    using Setting.Configuration;
    using Xunit;

    public class ProductStatusConfigurationHandlerTests
    {
        [Fact]
        public void UseConfig_Always_AssignsCorrectToStatusSettings()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(typeof(ProductStatusAuthorizeServiceForTesting))
                .UseUserService(typeof(ProductStatusUserServiceForTesting))
                .SetUnAuthorizedUrl("~/Error/UnAuthorized");

            // Act
            ProductStatusConfigurationHandler.UseConfig(configuration);

            // Assert
            Assert.True(
                null != ProductStatusConfigurationHandler.CreateAuthorizeService()
                && null != ProductStatusConfigurationHandler.CreateUserService()
                && null != ProductStatusConfigurationHandler.GetUnAuthorizedUrl());
        }

        [Fact]
        public void UseConfig_ProductStatusConfigurationIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductStatusConfigurationHandler.UseConfig(null));
        }

        [Fact]
        public void UseConfig_AuthorizeServiceTypeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration()
                .UseUserService(typeof(ProductStatusUserServiceForTesting))
                .SetUnAuthorizedUrl("~/Error/UnAuthorized");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductStatusConfigurationHandler.UseConfig(configuration));
        }

        [Fact]
        public void UseConfig_UserServiceTypeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(typeof(ProductStatusAuthorizeServiceForTesting))
                .SetUnAuthorizedUrl("~/Error/UnAuthorized");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductStatusConfigurationHandler.UseConfig(configuration));
        }

        [Fact]
        public void UseConfig_AuthorizedUrlIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(typeof(ProductStatusAuthorizeServiceForTesting))
                .UseUserService(typeof(ProductStatusUserServiceForTesting));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductStatusConfigurationHandler.UseConfig(configuration));
        }
    }
}
