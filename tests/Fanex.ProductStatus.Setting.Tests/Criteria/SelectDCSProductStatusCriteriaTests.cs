namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Xunit;

    public class SelectDCSProductStatusCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SelectDCSProductStatusCriteria(21, 1176083);
            var expectedSettingKey = "Acc_Admin_DisableNewProductStatusSelect";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
