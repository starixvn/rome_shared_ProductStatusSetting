namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Xunit;

    [Trait("Criteria", "AllowedDownlinesByLevelCriteria")]
    public class AllowedDownlinesByLevelCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new AllowedDownlinesByLevelCriteria(16, 4, true, true, true);
            var expectedSettingKey = "Acc_AdminCheckAllowedDownlinesByLevel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(0, false)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_ActorId_ReturnsExpectedResult(int actorId, bool expectedResult)
        {
            // Arrange
            var criteria = new AllowedDownlinesByLevelCriteria(actorId, 4, true, true, true);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(5, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        public void IsValid_CustRole_ReturnsExpectedResult(byte custRole, bool expectedResult)
        {
            // Arrange
            var criteria = new AllowedDownlinesByLevelCriteria(26, custRole, true, true, true);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
