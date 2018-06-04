namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Setting.Models;
    using Xunit;

    [Trait("Criteria", "UpdateGoldDeluxeStatusCriteria")]
    public class UpdateGoldDeluxeStatusCriteriaTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";
        private static readonly ProductStatusCommand CommandTest = new ProductStatusCommand
        {
            SportType = 255,
            BetType = 1700,
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
            var criteria = new UpdateGoldDeluxeStatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandTest);
            var expectedSettingKey = "Acc_AdminUpdDisableGoldDeluxe";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
