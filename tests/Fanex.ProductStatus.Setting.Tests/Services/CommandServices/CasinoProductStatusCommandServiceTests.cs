namespace Fanex.ProductStatus.Setting.Tests.Services.CommandServices
{
    using System.Linq;
    using DataAccess;
    using NAP.Framework.Data;
    using NSubstitute;
    using Setting.Models;
    using Setting.Services.CommandServices;
    using Setting.Shared;
    using Xunit;

    [Trait("Services", "CommandServices")]
    public class CasinoProductStatusCommandServiceTests
    {
        private const int CustIdTest = 1176083;
        private const int CustRoleTest = 3;
        private const int ActorIdTest = 8;
        private const string ActorNameTest = "sa";
        private const string IPTest = "127.0.0.1";

        private readonly ProductStatusDataAccess productStatusDataAccess;
        private readonly RngCasinoAPIWrapper rngCasinoAPIWrapper;
        private readonly CasinoProductStatusCommandService casinoProductStatusCommandService;

        public CasinoProductStatusCommandServiceTests()
        {
            productStatusDataAccess = Substitute.For<ProductStatusDataAccess>();
            rngCasinoAPIWrapper = Substitute.For<RngCasinoAPIWrapper>();
            casinoProductStatusCommandService = new CasinoProductStatusCommandService(productStatusDataAccess, rngCasinoAPIWrapper);
        }

        [Fact]
        public void UpdateProductStatus_UnsyncedRnGCasino_ReturnErrorResult()
        {
            // Arrange
            var commands = Enumerable.Empty<ProductStatusCommand>();

            var expectedErrorCode = 109;
            productStatusDataAccess.SyncedRnGCasino(Arg.Any<ICriteria>()).Returns(false);

            // Act
            var actualResult = casinoProductStatusCommandService.UpdateProductStatus(ProductIdentity.Casino, CustIdTest, CustRoleTest, ActorIdTest, ActorNameTest, IPTest, commands);

            // Assert
            Assert.Equal(actualResult.Code, expectedErrorCode);
        }

        [Fact]
        public void SynchronizeStatus_AllowDownlines_CallUpdateEnableCasinoByLevel()
        {
            // Arrange
            var commands = Enumerable.Empty<ProductStatusCommand>();

            var result = new Result(0, "successfully");
            productStatusDataAccess.ExecuteNonQuery(Arg.Any<ICriteria>()).Returns(result);

            // Act
            var actualResult = casinoProductStatusCommandService.SynchronizeStatus(
                CustIdTest,
                CustRoleTest, 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                IPTest,
                8,
                "sa",
                3);

            // Assert
            rngCasinoAPIWrapper.Received().UpdateEnableCasinoByLevel(CustIdTest,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                IPTest,
                8,
                "sa",
                3);
        }

        [Fact]
        public void SynchronizeStatus_DoesNotAllowDownlines_DoesNotCallUpdateEnableCasinoByLevel()
        {
            // Arrange
            var commands = Enumerable.Empty<ProductStatusCommand>();

            var result = new Result(103, "Does not allow downlines");
            productStatusDataAccess.ExecuteNonQuery(Arg.Any<ICriteria>()).Returns(result);

            // Act
            var actualResult = casinoProductStatusCommandService.SynchronizeStatus(
                CustIdTest,
                CustRoleTest,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                IPTest,
                8,
                "sa",
                3);

            // Assert
            rngCasinoAPIWrapper.DidNotReceive().UpdateEnableCasinoByLevel(CustIdTest,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                IPTest,
                8,
                "sa",
                3);
        }
    }
}
