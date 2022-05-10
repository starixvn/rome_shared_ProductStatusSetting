namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Setting.Shared;
    using Xunit;

    [Trait("Criteria", "SelectProductStatusCriteria")]
    public class SelectProductStatusCriteriaTests
    {
        private const int CustIdTest = 1176083;

        [Theory]
        [InlineData(ProductIdentity.NumberGame)]
        [InlineData(ProductIdentity.Casino)]
        [InlineData(ProductIdentity.Keno)]
        public void GetSettingKey_Always_ReturnsValidSettingKey(int productId)
        {
            // Arrange
            var criteria = new SelectProductStatusCriteria(productId, CustIdTest, 0);
            var expectedSettingKey = "Acc_AdminSelStatusByProductId";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(ProductIdentity.NumberGame)]
        [InlineData(ProductIdentity.Casino)]
        [InlineData(ProductIdentity.Keno)]
        public void IsValid_ValidCriteria_ReturnsTrue(int productId)
        {
            // Arrange
            var criteria = new SelectProductStatusCriteria(productId, CustIdTest, 0);
            var expectedResult = true;

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(0, false)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_ProductId_ReturnsExpectedResult(int productId, bool expectedResult)
        {
            // Arrange
            var criteria = new SelectProductStatusCriteria(productId, CustIdTest, 0);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(0, false)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_CustId_ReturnsExpectedResult(int custId, bool expectedResult)
        {
            // Arrange
            var criteria = new SelectProductStatusCriteria(ProductIdentity.NumberGame, custId, 0);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}