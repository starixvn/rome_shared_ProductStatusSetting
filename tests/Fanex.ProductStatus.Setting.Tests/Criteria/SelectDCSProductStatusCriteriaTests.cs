using Fanex.ProductStatus.Setting.Criteria;
using Xunit;

namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    [Trait("Category", "ProductStatus")]
    public class SelectDCSProductStatusCriteriaTests
    {
        private const int ProductIdTest = 21;
        private const int CustIdTest = 1176083;

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SelectDCSProductStatusCriteria(ProductIdTest, CustIdTest);
            var expectedSettingKey = "Acc_Admin_DisableNewProductStatusSelect";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new SelectDCSProductStatusCriteria(ProductIdTest, CustIdTest);
            var expectedResult = true;

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
