namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Shared;
    using Xunit;

    public class SelectProductInfoCriteriaTests
    {
        [Theory]
        [InlineData(ProductIdentity.Casino)]
        [InlineData(ProductIdentity.Keno)]
        [InlineData(ProductIdentity.NG2)]
        [InlineData(21)]
        public void GetSettingKey_Always_ReturnsValidSettingKey(int productId)
        {
            // Arrange
            var criteria = new SelectProductInfoCriteria(productId);
            var expectedSettingKey = "EAS_CustomerSelSportsByProduct";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(ProductIdentity.Keno)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidProductId_ReturnsTrue(int productId)
        {
            // Arrange
            var criteria = new SelectProductInfoCriteria(productId);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void IsValid_InvalidProductId_ReturnsFalse(int productId)
        {
            // Arrange
            var criteria = new SelectProductInfoCriteria(productId);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
