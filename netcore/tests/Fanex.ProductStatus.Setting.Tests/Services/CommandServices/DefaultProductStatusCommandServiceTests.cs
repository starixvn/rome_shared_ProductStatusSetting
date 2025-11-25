namespace Fanex.ProductStatus.Setting.Tests.Services.CommandServices
{
    using System.Collections.Generic;
    using AppResources;
    using DataAccess;
    // using NAP.Framework.Data; // TODO: Fanex library - user will handle later
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.Services.CommandServices;
    using Xunit;

    [Trait("Services", "CommandServices")]
    public class DefaultProductStatusCommandServiceTests
    {
        private const int CustIdTest = 1176083;
        private const int CustRoleTest = 3;
        private const int ActorIdTest = 8;
        private const string ActorNameTest = "sa";
        private const string IPTest = "127.0.0.1";

        private readonly ProductStatusDataAccess productStatusDataAccess;
        private readonly CriteriaFactoryService criteriaFactory;
        private readonly DefaultProductStatusCommandService productStatusCommand;

        public DefaultProductStatusCommandServiceTests()
        {
            productStatusDataAccess = Substitute.For<ProductStatusDataAccess>();
            criteriaFactory = Substitute.For<CriteriaFactoryService>();
            productStatusCommand = new DefaultProductStatusCommandService(productStatusDataAccess);
        }

        [Fact]
        public void UpdateProductStatus_ReturnsSucessfulResult()
        {
            // Arrange
            var commands = new List<ProductStatusCommand>
            {
                new ProductStatusCommand
                {
                    SportType = 211,
                    BetType = 2401,
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
                    SportType = 211,
                    BetType = 2402,
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
            var actualResult = productStatusCommand.UpdateProductStatus(21, CustIdTest, CustRoleTest, ActorIdTest, ActorNameTest, IPTest, 0, commands);

            // Assert
            Assert.True(actualResult.IsSuccess());
        }

        [Fact]
        public void UpdateProductStatus_Product_ReturnsErrorResult()
        {
            // Arrange
            var commands = new List<ProductStatusCommand>
            {
                new ProductStatusCommand
                {
                    SportType = 211,
                    BetType = 2401,
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
                    SportType = 211,
                    BetType = 2402,
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

            // Act
            var actualResult = productStatusCommand.UpdateProductStatus(21, CustIdTest, CustRoleTest, ActorIdTest, ActorNameTest, IPTest, 0, commands);

            // Assert
            Assert.False(errorResult.IsSuccess());
        }
    }
}
