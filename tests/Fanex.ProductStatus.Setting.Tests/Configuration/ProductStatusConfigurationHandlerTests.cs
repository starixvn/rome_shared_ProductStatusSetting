namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Setting.Configuration;
    using Xunit;

    [Trait("Configuration", "ProductStatusConfigurationHandler")]
    public class ProductStatusConfigurationHandlerTests
    {
        [Fact]
        public void UseConfig_Always_AssignsCorrectToStatusSettings()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(new ProductStatusAuthorizeServiceForTesting())
                .UseProductStatusActor(new ProductStatusActorForTesting())
                .SetUnAuthorizedUrl("~/Error/UnAuthorized");

            // Act
            ProductStatusConfigurationHandler.UseConfig(configuration);

            // Assert
            Assert.True(
                null != ProductStatusConfigurationHandler.AuthorizeService
                && null != ProductStatusConfigurationHandler.Actor
                && null != ProductStatusConfigurationHandler.UnAuthorizedUrl);
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
                .UseProductStatusActor(new ProductStatusActorForTesting())
                .SetUnAuthorizedUrl("~/Error/UnAuthorized");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductStatusConfigurationHandler.UseConfig(configuration));
        }

        [Fact]
        public void UseConfig_UserIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(new ProductStatusAuthorizeServiceForTesting())
                .SetUnAuthorizedUrl("~/Error/UnAuthorized");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductStatusConfigurationHandler.UseConfig(configuration));
        }

        [Fact]
        public void UseConfig_AuthorizedUrlIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(new ProductStatusAuthorizeServiceForTesting())
                .UseProductStatusActor(new ProductStatusActorForTesting());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductStatusConfigurationHandler.UseConfig(configuration));
        }

        [Fact]
        public void UseConfig_Always_AssignsCorrectToCustomerProductList()
        {
            // Arrange
            var configuration = new ProductStatusConfiguration()
                .UseAuthorizeService(new ProductStatusAuthorizeServiceForTesting())
                .UseProductStatusActor(new ProductStatusActorForTesting())
                .SetUnAuthorizedUrl("~/Error/UnAuthorized")
                .UseCustomerProductList(() => new List<Product> { new Product(3, "Number Game", false) });

            // Act
            ProductStatusConfigurationHandler.UseConfig(configuration);

            // Assert
            Assert.True(
                null != ProductStatusConfigurationHandler.AuthorizeService
                && null != ProductStatusConfigurationHandler.Actor
                && null != ProductStatusConfigurationHandler.UnAuthorizedUrl
                && 1 == ProductStatusConfigurationHandler.CustomerProductListGetter().Count());
        }
    }
}