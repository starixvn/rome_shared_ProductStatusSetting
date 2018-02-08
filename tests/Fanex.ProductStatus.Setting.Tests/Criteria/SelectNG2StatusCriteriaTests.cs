namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Shared;
    using Xunit;

    [Trait("Category", "ProductStatus")]
    public class SelectNG2StatusCriteriaTests
    {
        private const int CustIdTest = 1176083;

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SelectNG2StatusCriteria(ProductIdentity.NG2, CustIdTest);
            var expectedSettingKey = "Acc_Admin_DisableGameStatusSelect";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new SelectGoldDeluxeStatusCriteria(ProductIdentity.NG2, CustIdTest);
            var expectedResult = true;

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
