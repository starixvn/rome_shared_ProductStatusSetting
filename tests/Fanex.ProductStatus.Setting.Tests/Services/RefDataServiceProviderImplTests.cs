namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using AppResources;
    using NSubstitute;
    using Setting.Services;
    using Xunit;

    public class RefDataServiceProviderImplTests
    {
        private readonly RefDataService refDataService;
        private readonly RefDataServiceProviderImpl refDataServiceProvider;

        public RefDataServiceProviderImplTests()
        {
            refDataService = Substitute.For<RefDataService>();
            refDataServiceProvider = new RefDataServiceProviderImpl(refDataService);
        }

        [Fact]
        public void GetBetTypeName_2401_ReturnsCorrectValue()
        {
            // Arrang
            var expectedBetType = Resources.BetType_2401;

            // Act
            var actualBetType = refDataServiceProvider.GetBetTypeName(2401);

            // Assert
            Assert.Equal(expectedBetType, actualBetType);
        }

        [Fact]
        public void GetBetTypeName_ValidBetTypeId_ReturnsCorrectValue()
        {
            // Arrang
            var betTypeId = 1501;
            var expectedBetType = "Australia";
            refDataService
                .GetBetTypeName(betTypeId.ToString(), Arg.Any<string>())
                .Returns(expectedBetType);

            // Act
            var actualBetType = refDataServiceProvider.GetBetTypeName(betTypeId);

            // Assert
            Assert.Equal(expectedBetType, actualBetType);
        }

        [Fact]
        public void GetBetTypeName_InvalidBetTypeId_ReturnsEmpty()
        {
            // Arrang
            var betTypeId = -1;
            var expectedBetType = string.Empty;
            refDataService
                .GetBetTypeName(betTypeId.ToString(), Arg.Any<string>())
                .Returns(expectedBetType);

            // Act
            var actualBetType = refDataServiceProvider.GetBetTypeName(betTypeId);

            // Assert
            Assert.Equal(expectedBetType, actualBetType);
        }

        [Fact]
        public void GetProductName_ValidProductId_ReturnsCorrectValue()
        {
            // Arrang
            var productId = 30;
            var expectedProductName = "Bullfighting";
            refDataService
                .GetResourceDataByRefIdAndResourceId("Product", productId.ToString(), Arg.Any<string>())
                .Returns(expectedProductName);

            // Act
            var actualBetType = refDataServiceProvider.GetProductName(productId);

            // Assert
            Assert.Equal(expectedProductName, actualBetType);
        }

        [Fact]
        public void GetProductName_InvalidProductId_ReturnsEmpty()
        {
            // Arrang
            var productId = -1;
            var expectedProductName = string.Empty;
            refDataService
                .GetResourceDataByRefIdAndResourceId("Product", productId.ToString(), Arg.Any<string>())
                .Returns(expectedProductName);

            // Act
            var actualBetType = refDataServiceProvider.GetProductName(productId);

            // Assert
            Assert.Equal(expectedProductName, actualBetType);
        }
    }
}
