namespace Fanex.ProductStatus.Setting.Tests.Services.CommandServices
{
    using System.Collections.Generic;
    using AppResources;
    using DataAccess;
    using NAP.Framework.Data;
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.Services.CommandServices;
    using Setting.Shared;
    using Xunit;

    [Trait("Services", "CommandServices")]
    public class KenoProductStatusCommandServiceTests
    {
        private const int CustIdTest = 1176083;
        private const int CustRoleTest = 3;
        private const int ActorIdTest = 8;
        private const string ActorNameTest = "sa";
        private const string IPTest = "127.0.0.1";

        private readonly ProductStatusDataAccess productStatusDataAccess;
        private readonly RefDataService refDataService;
        private readonly KenoProductStatusCommandService kenoProductStatusCommand;

        public KenoProductStatusCommandServiceTests()
        {
            refDataService = Substitute.For<RefDataService>();
            productStatusDataAccess = Substitute.For<ProductStatusDataAccess>();
            kenoProductStatusCommand = new KenoProductStatusCommandService(productStatusDataAccess, refDataService);
        }

        [Fact]
        public void UpdateProductStatus_ReturnsSucessfulResult()
        {
            // Arrange
            var commands = new List<ProductStatusCommand>
            {
                new ProductStatusCommand
                {
                    SportType = 202,
                    BetType = 1501,
                    Disable = true,
                    UpdateAllMasters = true,
                    DisableAllMasters = true,
                    UpdateAllAgents = true,
                    DisableAllAgents = true,
                    UpdateAllMembers = true,
                    DisableAllMembers = true
                },
                new ProductStatusCommand
                {
                    SportType = 202,
                    BetType = 1503,
                    Disable = true,
                    UpdateAllMasters = true,
                    DisableAllMasters = true,
                    UpdateAllAgents = true,
                    DisableAllAgents = true,
                    UpdateAllMembers = true,
                    DisableAllMembers = true
                },
            };

            var successfulResult = new Result(0, Resources.StatusUpdatedSuccessfully);
            productStatusDataAccess
                .ExecuteNonQuery(Arg.Any<ICriteria>())
                .Returns(successfulResult);

            // Act
            var actualResult = kenoProductStatusCommand.UpdateProductStatus(ProductIdentity.Keno, CustIdTest, CustRoleTest, ActorIdTest, ActorNameTest, IPTest, commands);

            // Assert
            Assert.True(actualResult.IsSuccess());
        }

        [Fact]
        public void UpdateProductStatus_ReturnsFirstError()
        {
            // Arrange
            var commands = new List<ProductStatusCommand>
            {
                new ProductStatusCommand
                {
                    SportType = 202,
                    BetType = 1501,
                    Disable = true,
                    UpdateAllMasters = true,
                    DisableAllMasters = true,
                    UpdateAllAgents = true,
                    DisableAllAgents = true,
                    UpdateAllMembers = true,
                    DisableAllMembers = true
                },
                new ProductStatusCommand
                {
                    SportType = 202,
                    BetType = 1503,
                    Disable = true,
                    UpdateAllMasters = true,
                    DisableAllMasters = true,
                    UpdateAllAgents = true,
                    DisableAllAgents = true,
                    UpdateAllMembers = true,
                    DisableAllMembers = true
                },
            };

            var errorResult = new Result(102, null);
            productStatusDataAccess
                .ExecuteNonQuery(Arg.Any<ICriteria>())
                .Returns(errorResult);
            refDataService.GetBetTypeName(1501).Returns(string.Empty);
            refDataService.GetBetTypeName(1503).Returns(string.Empty);

            // Act
            var actualResult = kenoProductStatusCommand.UpdateProductStatus(ProductIdentity.Keno, CustIdTest, CustRoleTest, ActorIdTest, ActorNameTest, IPTest, commands);

            // Assert
            Assert.False(actualResult.IsSuccess());
        }

        [Fact]
        public void UpdateProductStatus_IsUM_ReturnsErrorMsgWithoutBetTypeName()
        {
            // Arrange
            var commands = new List<ProductStatusCommand>
            {
                new ProductStatusCommand
                {
                    SportType = 202,
                    BetType = 1501,
                    Disable = true,
                    UpdateAllMasters = true,
                    DisableAllMasters = true,
                    UpdateAllAgents = true,
                    DisableAllAgents = true,
                    UpdateAllMembers = true,
                    DisableAllMembers = true
                },
                new ProductStatusCommand
                {
                    SportType = 202,
                    BetType = 1503,
                    Disable = true,
                    UpdateAllMasters = true,
                    DisableAllMasters = true,
                    UpdateAllAgents = true,
                    DisableAllAgents = true,
                    UpdateAllMembers = true,
                    DisableAllMembers = true
                },
            };

            var errorResult = new Result(Result.UMNonDCSErrorCode, "Product is UM");
            productStatusDataAccess
                .ExecuteNonQuery(Arg.Any<ICriteria>())
                .Returns(errorResult);
            refDataService.GetBetTypeName(1501).Returns(string.Empty);
            refDataService.GetBetTypeName(1503).Returns(string.Empty);

            // Act
            var actualResult = kenoProductStatusCommand.UpdateProductStatus(ProductIdentity.Keno, CustIdTest, CustRoleTest, ActorIdTest, ActorNameTest, IPTest, commands);

            // Assert
            Assert.True(actualResult.IsUMOfNonDCS() && actualResult.Message == errorResult.Message);
        }
    }
}
