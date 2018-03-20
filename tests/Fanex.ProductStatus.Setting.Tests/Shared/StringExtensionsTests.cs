namespace Fanex.ProductStatus.Setting.Tests.Shared
{
    using Setting.Shared;
    using Xunit;

    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SplitString_IsNullOrEmpty_ReturnsEmptyArray(string value)
        {
            // Arrange & Act
            var actual = value.SplitString();

            // Assert
            Assert.True(actual.Length == 0);
        }

        [Fact]
        public void SplitString_ValidString_ReturnsArray()
        {
            // Arrange
            string value = "supertest1,supertest05,supertest10";

            // Act
            var actual = value.SplitString();

            // Assert
            Assert.True(actual.Length > 0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SplitInt_IsNullOrEmpty_ReturnsEmptyArray(string value)
        {
            // Arrange & Act
            var actual = value.SplitInt();

            // Assert
            Assert.True(actual.Length == 0);
        }

        [Fact]
        public void SplitInt_InvalidString_ReturnsEmptyArray()
        {
            // Arrange
            string value = "supertest1,supertest05,supertest10";

            // Act
            var actual = value.SplitInt();

            // Assert
            Assert.True(actual.Length == 0);
        }

        [Fact]
        public void SplitInt_ValidString_ReturnsArray()
        {
            // Arrange
            string value = "1,2,3,4,5,6,7,8,9";

            // Act
            var actual = value.SplitInt();

            // Assert
            Assert.True(actual.Length > 0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ToDictionary_IsNullOrEmpty_ReturnsEmptyDictionary(string value)
        {
            // Arrange & Act
            var actual = value.ToDictionary();

            // Assert
            Assert.True(actual.Count == 0);
        }

        [Fact]
        public void ToDictionary_ValidString_ReturnsDictionary()
        {
            // Arrange
            string value = "1,2,3,4,5,supertest,abc,5,4,3,2,1";

            // Act
            var actual = value.ToDictionary();

            // Assert
            Assert.True(actual.Count > 0);
        }
    }
}
