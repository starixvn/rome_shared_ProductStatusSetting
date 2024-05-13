namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Fanex.ProductStatus.Setting.Criteria;
    using Xunit;

    [Trait("Criteria", "CheckProducStatusUMCriteria")]
    public class CheckProducStatusUMCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new CheckProducStatusUMCriteria(22, 123458);
            var expectedSettingKey = "Acc_CheckUM_MultipleCustProductStatus";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(-22, false)]
        [InlineData(0, false)]
        [InlineData(22, true)]
        public void IsValid_ProductId_ReturnsCorrectExpectedResult(int productId, bool expected)
        {
            // Arrange
            var criteria = new CheckProducStatusUMCriteria(productId, 123458);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("123458", true)]
        public void IsValid_CustIds_ReturnsCorrectExpectedResult(string custIds, bool expected)
        {
            // Arrange
            var criteria = new CheckProducStatusUMCriteria(custIds, 22);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}