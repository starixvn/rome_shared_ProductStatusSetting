namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Setting.Models;
    using Xunit;

    public class UpdateNumberGameStatusCriteriaTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";
        private static readonly ProductStatusCommand CommandTest = new ProductStatusCommand
        {
            SportType = 161,
            BetType = 81,
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
            var criteria = new UpdateNumberGameStatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandTest);
            var expectedSettingKey = "Acc_AdminUpdDisableNumberGame";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
