namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using Setting.Criteria;
    using Setting.Models;
    using Xunit;

    public class UpdateDCSProductStatusCriteriaTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";
        private static readonly ProductStatusCommand[] CommandsTest = new[]
        {
            new ProductStatusCommand
            {
                SportType = 211,
                BetType = 2401,
                Disable = false,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = false,
                UpdateAllMembers = true,
                DisableAllMembers = false
            },
            new ProductStatusCommand
            {
                SportType = 211,
                BetType = 2402,
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
            var criteria = new UpdateDCSProductStatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandsTest);
            var expectedSettingKey = "Acc_Admin_DisableNewProductStatusUpd";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(0, false)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_CustId_ReturnsExpectedResult(int custId, bool expectedResult)
        {
            // Arrange
            var updateCriteriaBase = new UpdateDCSProductStatusCriteria(custId, ActorIdTest, IPTest, CommandsTest);

            // Act
            var actualResult = updateCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(0, false)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_ActorId_ReturnsExpectedResult(int actorId, bool expectedResult)
        {
            // Arrange
            var updateCriteriaBase = new UpdateDCSProductStatusCriteria(CustIdTest, actorId, IPTest, CommandsTest);

            // Act
            var actualResult = updateCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetSpParams_Always_ContainsCorrectData()
        {
            // Arrange
            var criteria = new UpdateDCSProductStatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandsTest);

            // Act
            var actualSpParams = criteria.GetSpParams();

            // Assert
            Assert.True(
                actualSpParams.ContainsKey("custid")
                && actualSpParams.ContainsKey("isdisabled")
                && actualSpParams.ContainsKey("userid")
                && actualSpParams.ContainsKey("ip"));
        }

        [Fact]
        public void GetSpParams_Always_ReturnsCorrectIsDisabledXmlFormat()
        {
            // Arrange
            var criteria = new UpdateDCSProductStatusCriteria(CustIdTest, ActorIdTest, IPTest, CommandsTest);
            var expectedIsDisabledXml =
                "<ROOT>"
                + "<ProductStatus SportType=\"211\" BetType=\"2401\" IsDisabled=\"0\" UpdateAllMasters=\"1\" UpdateAllAgents=\"1\" UpdateAllMembers=\"1\" DisableAllMasters=\"0\" DisableAllAgents=\"0\" DisableAllMembers=\"0\"/>"
                + "<ProductStatus SportType=\"211\" BetType=\"2402\" IsDisabled=\"0\" UpdateAllMasters=\"1\" UpdateAllAgents=\"1\" UpdateAllMembers=\"1\" DisableAllMasters=\"0\" DisableAllAgents=\"0\" DisableAllMembers=\"0\"/>"
                + "</ROOT>";

            // Act
            var actualSpParams = criteria.GetSpParams();
            var actualIsDisabledXml = actualSpParams["isdisabled"];

            // Assert
            Assert.Equal(expectedIsDisabledXml, actualIsDisabledXml);
        }
    }
}
