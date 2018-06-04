namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Setting.Models;
    using Xunit;

    [Trait("Criteria", "UpdateNG2StatusCriteria")]
    public class UpdateNG2StatusCriteriaTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";
        private static readonly ProductStatusCommand[] CommandsTest = new[]
        {
            new ProductStatusCommand
            {
                SportType = 213,
                BetType = 2501,
                Disable = false,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = false,
                UpdateAllMembers = true,
                DisableAllMembers = false
            }
        };

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateNG2StatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandsTest);
            var expectedSettingKey = "Acc_Admin_DisableGameStatusUpd";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
