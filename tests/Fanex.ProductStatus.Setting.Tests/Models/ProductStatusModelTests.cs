namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using Setting.Models;
    using Xunit;

    public class ProductStatusModelTests
    {
        [Fact]
        public void New_Always_AssignCorrectProperties()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                false,
                10,
                100,
                20,
                200,
                30,
                300);

            // Assert
            Assert.True(
                161 == actualResult.SportType
                && 81 == actualResult.BetType
                && !actualResult.IsDisabled
                && !actualResult.IsDisabledUpline
                && 10 == actualResult.Master.TotalDisabled
                && 100 == actualResult.Master.TotalEnabled
                && 20 == actualResult.Agent.TotalDisabled
                && 200 == actualResult.Agent.TotalEnabled
                && 30 == actualResult.Member.TotalDisabled
                && 300 == actualResult.Member.TotalEnabled);
        }
    }
}
