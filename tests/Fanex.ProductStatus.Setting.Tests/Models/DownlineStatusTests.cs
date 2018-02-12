namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using Setting.Models;
    using Xunit;

    public class DownlineStatusTests
    {
        [Theory]
        [InlineData(0, 100)]
        [InlineData(200, 0)]
        [InlineData(0, 0)]
        public void New_Always_AssignCorrectProperties(int totalDisabled, int totalEnabled)
        {
            // Arrange && Act
            var actual = new DownlineStatus(false, totalDisabled, totalEnabled);

            // Assert
            Assert.True(
                totalDisabled == actual.TotalDisabled
                && totalEnabled == actual.TotalEnabled);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(200, 0)]
        [InlineData(0, 0)]
        public void Total_Always_ReturnsCorrectValue(int totalDisabled, int totalEnabled)
        {
            // Arrange
            var expected = totalDisabled + totalEnabled;

            // Act
            var actual = new DownlineStatus(false, totalDisabled, totalEnabled);

            // Assert
            Assert.Equal(expected, actual.Total);
        }

        [Theory]
        [InlineData(true, 100, 200)]
        [InlineData(false, 0, 0)]
        public void IsDisabled_Always_ReturnsTrue(bool isDisabledUpline, int totalDisabled, int totalEnabled)
        {
            // Arrange & Act
            var actual = new DownlineStatus(isDisabledUpline, totalDisabled, totalEnabled);

            // Assert
            Assert.True(actual.IsDisabled);
        }

        [Fact]
        public void IsDisabled_Always_ReturnsFalse()
        {
            // Arrange & Act
            var actual = new DownlineStatus(false, 100, 200);

            // Assert
            Assert.False(actual.IsDisabled);
        }
    }
}
