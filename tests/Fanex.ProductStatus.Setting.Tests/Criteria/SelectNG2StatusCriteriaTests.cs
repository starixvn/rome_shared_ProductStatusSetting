namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Shared;
    using Xunit;

    public class SelectNG2StatusCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SelectNG2StatusCriteria(ProductIdentity.NG2, 1176083);
            var expectedSettingKey = "Acc_Admin_DisableGameStatusSelect";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
