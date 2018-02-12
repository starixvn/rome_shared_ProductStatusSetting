namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using Setting.Models;
    using Shared;
    using Xunit;

    public class ProductStatusModelTests
    {
        [Fact]
        public void New_IsDisabledUplineIsTrue_ReturnsIsDisableIsTrue()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                true,
                0,
                0,
                0,
                0,
                0,
                0,
                RoleIdentity.Member);

            // Assert
            Assert.True(actualResult.IsDisabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void New_IsDisabledUplineIsFalse_ReturnsCorrectIsDisabled(bool isDisabled)
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                isDisabled,
                false,
                0,
                0,
                0,
                0,
                0,
                0,
                RoleIdentity.Member);

            // Assert
            Assert.Equal(isDisabled, actualResult.IsDisabled);
        }

        [Fact]
        public void New_Member_ReturnsMasterAgentMemberAreNull()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                false,
                0,
                0,
                0,
                0,
                0,
                0,
                RoleIdentity.Member);

            // Assert
            Assert.True(
                161 == actualResult.SportType
                && 81 == actualResult.BetType
                && !actualResult.IsDisabled
                && !actualResult.IsDisabledUpline
                && null == actualResult.Master
                && null == actualResult.Agent
                && null == actualResult.Member);
        }

        [Fact]
        public void New_Agent_ReturnsMasterAgentAreNull()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                false,
                0,
                0,
                0,
                0,
                10,
                100,
                RoleIdentity.Agent);

            // Assert
            Assert.True(
                161 == actualResult.SportType
                && 81 == actualResult.BetType
                && !actualResult.IsDisabled
                && !actualResult.IsDisabledUpline
                && null == actualResult.Master
                && null == actualResult.Agent
                && 10 == actualResult.Member.TotalDisabled
                && 100 == actualResult.Member.TotalEnabled);
        }

        [Fact]
        public void New_Master_ReturnsMasterIsNull()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                false,
                0,
                0,
                20,
                200,
                10,
                100,
                RoleIdentity.Master);

            // Assert
            Assert.True(
                161 == actualResult.SportType
                && 81 == actualResult.BetType
                && !actualResult.IsDisabled
                && !actualResult.IsDisabledUpline
                && null == actualResult.Master
                && 20 == actualResult.Agent.TotalDisabled
                && 200 == actualResult.Agent.TotalEnabled
                && 10 == actualResult.Member.TotalDisabled
                && 100 == actualResult.Member.TotalEnabled);
        }

        [Fact]
        public void New_Master_ReturnsMemberIsDisabled()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                false,
                0,
                0,
                20,
                0,
                10,
                100,
                RoleIdentity.Master);

            // Assert
            Assert.True(actualResult.Member.IsDisabled);
        }

        [Fact]
        public void New_Super_ReturnsCorrectProperties()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                false,
                30,
                300,
                20,
                200,
                10,
                100,
                RoleIdentity.Super);

            // Assert
            Assert.True(
                161 == actualResult.SportType
                && 81 == actualResult.BetType
                && !actualResult.IsDisabled
                && !actualResult.IsDisabledUpline
                && 30 == actualResult.Master.TotalDisabled
                && 300 == actualResult.Master.TotalEnabled
                && 20 == actualResult.Agent.TotalDisabled
                && 200 == actualResult.Agent.TotalEnabled
                && 10 == actualResult.Member.TotalDisabled
                && 100 == actualResult.Member.TotalEnabled);
        }

        [Fact]
        public void New_Super_ReturnsAgentIsDisabled()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                false,
                30,
                0,
                20,
                200,
                10,
                100,
                RoleIdentity.Super);

            // Assert
            Assert.True(actualResult.Agent.IsDisabled);
        }

        [Fact]
        public void New_Super_ReturnsMemberIsDisabled()
        {
            // Arrange && Act
            var actualResult = new ProductStatusModel(
                161,
                81,
                false,
                false,
                30,
                300,
                20,
                0,
                10,
                100,
                RoleIdentity.Super);

            // Assert
            Assert.True(actualResult.Member.IsDisabled);
        }
    }
}
