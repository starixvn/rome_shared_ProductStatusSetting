namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using System.Collections.Generic;
    using Setting.Models;
    using Setting.Shared;
    using Xunit;

    [Trait("Models", "ProductStatusCommand")]
    public class ProductStatusCommandTests
    {
        [Fact]
        public void CorrectProperties_Super_ReturnsCorrectProperties()
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 211,
                BetType = 2401,
                Disable = true,
                UpdateAllMasters = false,
                DisableAllMasters = false,
                UpdateAllAgents = false,
                DisableAllAgents = false,
                UpdateAllMembers = false,
                DisableAllMembers = false
            };

            // Act
            command.CorrectProperties(RoleIdentity.Super);

            // Act & Assert
            Assert.True(
                command.UpdateAllMasters
                && command.DisableAllMasters
                && command.UpdateAllAgents
                && command.DisableAllAgents
                && command.UpdateAllMembers
                && command.DisableAllMembers);
        }

        [Fact]
        public void CorrectProperties_Master_ReturnsCorrectProperties()
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 211,
                BetType = 2401,
                Disable = true,
                UpdateAllAgents = false,
                DisableAllAgents = false,
                UpdateAllMembers = false,
                DisableAllMembers = false
            };

            // Act
            command.CorrectProperties(RoleIdentity.Master);

            // Act & Assert
            Assert.True(command.UpdateAllAgents
                && command.DisableAllAgents
                && command.UpdateAllMembers
                && command.DisableAllMembers);
        }

        [Fact]
        public void CorrectProperties_Agent_ReturnsCorrectProperties()
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 211,
                BetType = 2401,
                Disable = true,
                UpdateAllMembers = false,
                DisableAllMembers = false
            };

            // Act
            command.CorrectProperties(RoleIdentity.Agent);

            // Act & Assert
            Assert.True(command.UpdateAllMembers
                && command.DisableAllMembers);
        }
    }
}
