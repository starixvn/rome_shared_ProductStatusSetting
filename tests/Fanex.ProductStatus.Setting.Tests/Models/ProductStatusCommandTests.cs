namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using System.Collections;
    using AppResources;
    using Setting.Models;
    using Xunit;

    public class ProductStatusCommandTests
    {

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]

        [InlineData(false, true, true)]
        [InlineData(false, false, true)]
        public void ValidAgent_ReturnsExpectedResult(
            bool disableAgent,
            bool disableAllMembers,
            bool expectedResult)
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                CustRole = 2,
                SportType = 211,
                BetType = 2401,
                Disable = disableAgent,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = true,
                UpdateAllMembers = true,
                DisableAllMembers = disableAllMembers
            };

            // Act
            var actualResult = command.ValidAgent();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, true, true, true)]

        [InlineData(false, false, true, true)]
        [InlineData(false, false, false, true)]
        [InlineData(false, true, false, true)]
        public void ValidMaster_ReturnsExpectedResult(
            bool disableMaster,
            bool disableAllAgents,
            bool disableAllMembers,
            bool expectedResult)
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                CustRole = 3,
                SportType = 211,
                BetType = 2401,
                Disable = disableMaster,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = disableAllAgents,
                UpdateAllMembers = true,
                DisableAllMembers = disableAllMembers
            };

            // Act
            var actualResult = command.ValidMaster();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(true, true, true, true, true)]

        [InlineData(true, true, true, false, false)]
        [InlineData(true, true, false, false, false)]

        [InlineData(true, false, true, true, false)]
        [InlineData(true, false, true, false, false)]
        [InlineData(true, false, false, true, false)]
        [InlineData(true, false, false, false, false)]

        [InlineData(false, true, true, true, true)]
        [InlineData(false, true, true, false, true)]
        [InlineData(false, true, false, false, true)]

        [InlineData(false, false, true, true, true)]
        [InlineData(false, false, true, false, true)]

        [InlineData(false, false, false, true, true)]
        [InlineData(false, false, false, false, true)]
        public void ValidSuper_ReturnsExpectedResult(
            bool disableSuper,
            bool disableAllMasters,
            bool disableAllAgents,
            bool disableAllMembers,
            bool expectedResult)
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                CustRole = 4,
                SportType = 211,
                BetType = 2401,
                Disable = disableSuper,
                UpdateAllMasters = true,
                DisableAllMasters = disableAllMasters,
                UpdateAllAgents = true,
                DisableAllAgents = disableAllAgents,
                UpdateAllMembers = true,
                DisableAllMembers = disableAllMembers
            };

            // Act
            var actualResult = command.ValidSuper();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
