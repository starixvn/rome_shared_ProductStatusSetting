namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using AppResources;
    using Setting.Services;
    using Xunit;

    public class RefDataServiceImplTests
    {
        private readonly RefDataServiceImpl refDataService;

        public RefDataServiceImplTests()
        {
            refDataService = new RefDataServiceImpl();
        }

        [Fact]
        public void GetBetTypeName_2401_ReturnsCorrectValue()
        {
            // Arrang
            var expectedBetType = Resources.BetType_2401;

            // Act
            var actualBetType = refDataService.GetBetTypeName(2401);

            // Assert
            Assert.Equal(expectedBetType, actualBetType);
        }
    }
}
