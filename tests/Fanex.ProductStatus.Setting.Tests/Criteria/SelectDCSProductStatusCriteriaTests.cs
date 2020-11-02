namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Xunit;

    [Trait("Criteria", "SelectDCSProductStatusCriteria")]
    public class SelectDCSProductStatusCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SelectDCSProductStatusCriteria(21, 1176083, 0);
            var expectedSettingKey = "Acc_Admin_DisableNewProductStatusSelect";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
