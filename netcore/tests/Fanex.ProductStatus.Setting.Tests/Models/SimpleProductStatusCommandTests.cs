namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using Setting.Models;
    using Setting.Shared;
    using Xunit;

    [Trait("Models", "SimpleProductStatusCommand")]
    public class SimpleProductStatusCommandTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToProductStatusCommand_Super_ReturnsCorrectProperties(bool disableDownlines)
        {
            // Arrange
            var command = new SimpleProductStatusCommand
            {
                SportType = 211,
                BetType = 2401,
                Disable = false,
                DisableDownlines = disableDownlines
            };

            // Act
            var actual = command.ToProductStatusCommand(RoleIdentity.Super);

            // Act & Assert
            Assert.True(
                actual.UpdateAllMasters
                && actual.DisableAllMasters == disableDownlines
                && actual.UpdateAllAgents
                && actual.DisableAllAgents == disableDownlines
                && actual.UpdateAllMembers
                && actual.DisableAllMembers == disableDownlines);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToProductStatusCommand_Master_ReturnsCorrectProperties(bool disableDownlines)
        {
            // Arrange
            var command = new SimpleProductStatusCommand
            {
                SportType = 211,
                BetType = 2401,
                Disable = false,
                DisableDownlines = disableDownlines
            };

            // Act
            var actual = command.ToProductStatusCommand(RoleIdentity.Master);

            // Act & Assert
            Assert.True(actual.UpdateAllAgents
                && actual.DisableAllAgents == disableDownlines
                && actual.UpdateAllMembers
                && actual.DisableAllMembers == disableDownlines);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToProductStatusCommand_Agent_ReturnsCorrectProperties(bool disableDownlines)
        {
            // Arrange
            var command = new SimpleProductStatusCommand
            {
                SportType = 211,
                BetType = 2401,
                Disable = false,
                DisableDownlines = disableDownlines
            };

            // Act
            var actual = command.ToProductStatusCommand(RoleIdentity.Agent);

            // Act & Assert
            Assert.True(actual.UpdateAllMembers
                && actual.DisableAllMembers == disableDownlines);
        }

        [Fact]
        public void ToProductStatusCommand_Disable_ReturnsCorrectProperties()
        {
            // Arrange
            var command = new SimpleProductStatusCommand
            {
                SportType = 211,
                BetType = 2401,
                Disable = true,
                DisableDownlines = false
            };

            // Act
            var actual = command.ToProductStatusCommand(RoleIdentity.Super);

            // Act & Assert
            Assert.True(
                actual.UpdateAllMasters
                && actual.DisableAllMasters
                && actual.UpdateAllAgents
                && actual.DisableAllAgents
                && actual.UpdateAllMembers
                && actual.DisableAllMembers);
        }
    }
}
