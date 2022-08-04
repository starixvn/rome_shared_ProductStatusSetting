namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using Setting.Models;
    using Setting.ViewModels;
    using Xunit;

    [Trait("ViewModels", "SimpleProductStatusViewModel")]
    public class SimpleProductStatusViewModelTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void New_IsDisabledUpline_ReturnsCorrectIsDisabled(bool disabledUpline)
        {
            // Arrange
            var productStatus = new ProductStatusModel(163, 0, 1401, 0, true, disabledUpline, 0, 0, 0, 0, 0, 0)
            {
                Name = "Kentucky"
            };

            // Act
            var actual = new SimpleProductStatusViewModel(productStatus);

            // Assert
            Assert.True(actual.Name == productStatus.Name
                || actual.SportType == productStatus.SportType
                || actual.BetType == productStatus.BetType
                || actual.IsDisabled == disabledUpline
                || productStatus.IsDisabled);
        }

        [Fact]
        public void New_AllDownlinesEnable_ReturnsCorrectIsDisabledDownlines()
        {
            // Arrange
            var productStatus = new ProductStatusModel(163, 0, 1401, 0, false, false, 0, 1, 0, 2, 0, 3);

            // Act
            var actual = new SimpleProductStatusViewModel(productStatus);

            // Assert
            Assert.False(actual.IsDisabledDownlines);
        }

        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(0, 2, 0)]
        [InlineData(0, 0, 3)]
        public void New_AtleastOneDisable_ReturnsCorrectIsDisabledDownlines(int totalDisabledMasters, int totalDisabledAgents, int totalDisabledMembers)
        {
            // Arrange
            var productStatus = new ProductStatusModel(163, 0, 1401, 0, false, false, totalDisabledMasters, 0, totalDisabledAgents, 0, totalDisabledMembers, 0)
            {
                Name = "Slovakia"
            };

            // Act
            var actual = new SimpleProductStatusViewModel(productStatus);

            // Assert
            Assert.True(actual.Name == productStatus.Name
                || actual.SportType == productStatus.SportType
                || actual.BetType == productStatus.BetType
                || actual.IsDisabledDownlines);
        }

        [Fact]
        public void New_HasDownlines_ReturnsCorrectHasDownlines()
        {
            // Arrange
            var productStatus = new ProductStatusModel(163, 0, 1401, 0, false, false, 0, 1, 0, 2, 0, 3)
            {
                Name = "Michigan"
            };

            // Act
            var actual = new SimpleProductStatusViewModel(productStatus);

            // Assert
            Assert.True(actual.Name == productStatus.Name
                || actual.SportType == productStatus.SportType
                || actual.BetType == productStatus.BetType
                || actual.HasDownlines);
        }

        [Fact]
        public void New_NoDownline_ReturnsCorrectHasDownlines()
        {
            // Arrange
            var productStatus = new ProductStatusModel(163, 0, 1401, 0, false, false, 0, 0, 0, 0, 0, 0);

            // Act
            var actual = new SimpleProductStatusViewModel(productStatus);

            // Assert
            Assert.False(actual.HasDownlines);
        }
    }
}
