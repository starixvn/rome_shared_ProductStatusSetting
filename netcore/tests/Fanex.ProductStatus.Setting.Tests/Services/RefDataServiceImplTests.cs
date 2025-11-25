namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using AppResources;
    using NSubstitute;
    // using RemotingRef; // TODO: Fanex library - user will handle later
    using Setting.Services;
    using Xunit;

    [Trait("Services", "RefDataServiceProviderImpl")]
    public class RefDataServiceImplTests
    {
        // TODO: Fanex RemotingRef library - user will handle later
        // private readonly IRemotingRefService remotingRefService;
        private readonly RefDataService refDataService;

        public RefDataServiceImplTests()
        {
            // remotingRefService = Substitute.For<IRemotingRefService>();
            // refDataService = new RefDataServiceImpl(remotingRefService);
            refDataService = null; // TODO: Fix after RemotingRef is available
        }       

        [Fact]
        public void GetBetTypeName_ValidBetTypeId_ReturnsCorrectValue()
        {
            // Arrang
            var betTypeId = 1501;
            var expectedBetType = "Australia";
            remotingRefService
                .GetBetTypeName(betTypeId.ToString(), "en-US")
                .Returns(expectedBetType);

            // Act
            var actualBetType = refDataService.GetBetTypeName(betTypeId);

            // Assert
            Assert.Equal(expectedBetType, actualBetType);
        }

        [Fact]
        public void GetBetTypeName_InvalidBetTypeId_ReturnsEmpty()
        {
            // Arrang
            var betTypeId = -1;
            var expectedBetType = string.Empty;
            remotingRefService
                .GetBetTypeName(betTypeId.ToString(), "en-US")
                .Returns(expectedBetType);

            // Act
            var actualBetType = refDataService.GetBetTypeName(betTypeId);

            // Assert
            Assert.Equal(expectedBetType, actualBetType);
        }

        [Fact]
        public void GetSportTypeName_ValidSportTypeId_ReturnsCorrectValue()
        {
            // Arrang
            var sportTypeId = 4;
            var expectedSportType = "Allbets";
            remotingRefService
                .GetSportName(sportTypeId.ToString(), "en-US", null)
                .Returns(expectedSportType);

            // Act
            var actualSportType = refDataService.GetSportTypeName(sportTypeId);

            // Assert
            Assert.Equal(expectedSportType, actualSportType);
        }

        [Fact]
        public void GetSportTypeName_InvalidSportTypeId_ReturnsEmpty()
        {
            // Arrang
            var sportTypeId = -1;
            var expectedSportType = string.Empty;
            remotingRefService
                .GetSportName(sportTypeId.ToString(), "en-US")
                .Returns(expectedSportType);

            // Act
            var actualSportType = refDataService.GetSportTypeName(sportTypeId);

            // Assert
            Assert.Equal(expectedSportType, actualSportType);
        }

        [Fact]
        public void GetProductName_ValidProductId_ReturnsCorrectValue()
        {
            // Arrang
            var productId = 30;
            var expectedProductName = "Bullfighting";
            remotingRefService
                .GetResourceDataByRefIdAndResourceId("Product", productId.ToString(), "en-US", string.Empty)
                .Returns(expectedProductName);

            // Act
            var actualBetType = refDataService.GetProductName(productId);

            // Assert
            Assert.Equal(expectedProductName, actualBetType);
        }

        [Fact]
        public void GetProductName_InvalidProductId_ReturnsEmpty()
        {
            // Arrang
            var productId = -1;
            var expectedProductName = string.Empty;
            remotingRefService
                .GetResourceDataByRefIdAndResourceId("Product", productId.ToString(), "en-US", string.Empty)
                .Returns(expectedProductName);

            // Act
            var actualBetType = refDataService.GetProductName(productId);

            // Assert
            Assert.Equal(expectedProductName, actualBetType);
        }
    }
}
