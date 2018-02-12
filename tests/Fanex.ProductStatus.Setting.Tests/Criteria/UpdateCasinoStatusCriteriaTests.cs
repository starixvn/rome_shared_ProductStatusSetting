namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Setting.Models;
    using Xunit;

    public class UpdateCasinoStatusCriteriaTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";
        private static readonly ProductStatusCommand CommandTest = new ProductStatusCommand
        {
            SportType = 208,
            BetType = 2101,
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
            var criteria = new UpdateCasinoStatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandTest);
            var expectedSettingKey = "Acc_AdminUpdCSStatus_Cust";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void GetSpParams_Always_ContainsDisabledCSField()
        {
            // Arrange
            var criteria = new UpdateCasinoStatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandTest);

            // Act
            var actualSpParams = criteria.GetSpParams();

            // Assert
            Assert.True(actualSpParams.ContainsKey("disabledcs"));
        }
    }
}
