namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Setting.Models;
    using Xunit;

    [Trait("Criteria", "UpdateKenoStatusCriteria")]
    public class UpdateKenoStatusCriteriaTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";
        private static readonly ProductStatusCommand CommandTest = new ProductStatusCommand
        {
            SportType = 202,
            BetType = 1501,
            Disable = false,
            UpdateAllMasters = true,
            DisableAllMasters = false,
            UpdateAllAgents = true,
            DisableAllAgents = false,
            UpdateAllMembers = true,
            DisableAllMembers = false
        };

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandTest);
            var expectedSettingKey = "Acc_AdminUpdDisableKeno";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(0, false)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_BetType_ReturnsExpectedResult(int betType, bool expectedResult)
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 202,
                BetType = betType,
                Disable = false,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = false,
                UpdateAllMembers = true,
                DisableAllMembers = false
            };
            var criteria = new UpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, command);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
