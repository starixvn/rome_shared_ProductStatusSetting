namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using DataAccess;
    using KellermanSoftware.CompareNetObjects;
    using NAP.Framework.Data;
    using NSubstitute;
    using Setting.Criteria;
    using Setting.Models;
    using Setting.Services;
    using Shared;
    using Xunit;

    public class ProductStatusServiceImplTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";

        private readonly ProductStatusDataAccess productStatusDataAccess;
        private readonly CriteriaFactoryService criteriaFactory;
        private readonly RefDataServiceProvider refDataServiceProvider;
        private readonly ProductStatusServiceImpl productStatusService;

        public ProductStatusServiceImplTests()
        {
            productStatusDataAccess = Substitute.For<ProductStatusDataAccess>();
            criteriaFactory = Substitute.For<CriteriaFactoryService>();
            refDataServiceProvider = Substitute.For<RefDataServiceProvider>();

            productStatusService = new ProductStatusServiceImpl(productStatusDataAccess, criteriaFactory, refDataServiceProvider);
        }

        [Theory]
        [InlineData(ProductIdentity.NumberGame, "Number Game")]
        [InlineData(ProductIdentity.Casino, "Casino")]
        [InlineData(ProductIdentity.GoldDeluxe, "Gold Deluxe")]
        [InlineData(ProductIdentity.ColossusBets, "Colossus Bets")]
        public void GetProductStatus_SingleRow_ReturnsDataFromDataAccess(int productId, string productName)
        {
            // Arrange
            var expectedResults = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    1,
                    1,
                    false,
                    false,
                    10,
                    100,
                    20,
                    200,
                    30,
                    300)
            };
            var criteria = new SelectProductStatusCriteria(productId, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(productId, CustIdTest)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria)
                .Returns(expectedResults);

            refDataServiceProvider
                .GetProductName(productId)
                .Returns(productName);

            var compareLogic = new CompareLogic();

            // Act
            var actualData = productStatusService.GetProductStatus(productId, CustIdTest);
            var actualResults = actualData.ToList();

            // Assert
            Assert.True(compareLogic.Compare(expectedResults, actualResults).AreEqual);
        }

        [Fact]
        public void GetProductStatus_Keno_ReturnsDataFromDataAccess()
        {
            // Arrange
            var expectedResults = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    202,
                    1501,
                    false,
                    false,
                    10,
                    100,
                    20,
                    200,
                    30,
                    300),
                new ProductStatusModel(
                    202,
                    1503,
                    false,
                    false,
                    10,
                    100,
                    20,
                    200,
                    30,
                    300)
            };
            var criteria = new SelectProductStatusCriteria(ProductIdentity.Keno, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(ProductIdentity.Keno, CustIdTest)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria)
                .Returns(expectedResults);

            refDataServiceProvider
                .GetBetTypeName(1501)
                .Returns("Australia");
            refDataServiceProvider
                .GetBetTypeName(1503)
                .Returns("Slovakia");

            var compareLogic = new CompareLogic();

            // Act
            var actualResults = productStatusService.GetProductStatus(ProductIdentity.Keno, CustIdTest);

            // Assert
            Assert.True(compareLogic.Compare(expectedResults, actualResults.ToList()).AreEqual);
        }

        [Fact]
        public void GetProductStatus_DCSMultipleRows_ReturnsDataFromDataAccess()
        {
            // Arrange
            var expectedResults = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    211,
                    2401,
                    false,
                    false,
                    10,
                    100,
                    20,
                    200,
                    30,
                    300),
                new ProductStatusModel(
                    211,
                    2402,
                    false,
                    false,
                    10,
                    100,
                    20,
                    200,
                    30,
                    300)
            };
            var criteria = new SelectDCSProductStatusCriteria(21, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(21, CustIdTest)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria)
                .Returns(expectedResults);

            refDataServiceProvider
                .GetBetTypeName(2401)
                .Returns("Baccarat, Roulette, Sic Bo, Dragon Tiger");
            refDataServiceProvider
                .GetBetTypeName(2402)
                .Returns("VIP Baccarat");

            // Act
            var actualData = productStatusService.GetProductStatus(21, CustIdTest);

            // Assert
            Assert.True(expectedResults.Count == actualData.Count());
        }

        [Fact]
        public void GetProductStatus_DCSSingleRow_ReturnsDataFromDataAccess()
        {
            // Arrange
            var expectedResults = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    220,
                    1021,
                    false,
                    false,
                    10,
                    100,
                    20,
                    200,
                    30,
                    300)
            };
            var criteria = new SelectDCSProductStatusCriteria(28, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(28, CustIdTest)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria)
                .Returns(expectedResults);

            refDataServiceProvider
                .GetProductName(28)
                .Returns("Lottery");

            var compareLogic = new CompareLogic();

            // Act
            var actualResults = productStatusService.GetProductStatus(28, CustIdTest);

            // Assert
            Assert.True(compareLogic.Compare(expectedResults, actualResults.ToList()).AreEqual);
        }

        [Fact]
        public void UpdateProductStatus_Keno_ReturnsSucessfulResult()
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
                
            var criteria = new List<UpdateKenoStatusCriteria>
            {
                new UpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands[0]),
                new UpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands[1])
            };
            criteriaFactory
                .CreateUpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands)
                .Returns(criteria);

            var successfulResult = new Result(0, Resources.StatusUpdatedSuccessfully);
            productStatusDataAccess
                .ExecuteNonQuery(Arg.Any<ICriteria>())
                .Returns(successfulResult);

            // Act
            var actualResult = productStatusService.UpdateProductStatus(ProductIdentity.Keno, CustIdTest, ActorIdTest, IPTest, commands);

            // Assert
            Assert.True(actualResult.IsSuccess());
        }

        [Fact]
        public void UpdateProductStatus_Keno_ReturnsFirstError()
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

            var criteria = new List<UpdateKenoStatusCriteria>
            {
                new UpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands[0]),
                new UpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands[1])
            };
            criteriaFactory
                .CreateUpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands)
                .Returns(criteria);

            var errorResult = new Result(102, null);
            productStatusDataAccess
                .ExecuteNonQuery(Arg.Any<ICriteria>())
                .Returns(errorResult);

            // Act
            var actualResult = productStatusService.UpdateProductStatus(ProductIdentity.Keno, CustIdTest, ActorIdTest, IPTest, commands);

            // Assert
            Assert.False(actualResult.IsSuccess());
        }

        [Fact]
        public void UpdateProductStatus_Product_ReturnsSucessfulResult()
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

            var criteria = new UpdateDCSProductStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands);
            criteriaFactory
                .CreateUpdateCriteria(21, CustIdTest, ActorIdTest, IPTest, commands)
                .Returns(criteria);

            var successfulResult = new Result(0, Resources.StatusUpdatedSuccessfully);
            productStatusDataAccess
                .ExecuteNonQuery(criteria)
                .Returns(successfulResult);

            // Act
            var actualResult = productStatusService.UpdateProductStatus(21, CustIdTest, ActorIdTest, IPTest, commands);

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

            var criteria = new UpdateDCSProductStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands);
            criteriaFactory
                .CreateUpdateCriteria(21, CustIdTest, ActorIdTest, IPTest, commands)
                .Returns(criteria);

            var errorResult = new Result(102, null);
            productStatusDataAccess
                .ExecuteNonQuery(criteria)
                .Returns(errorResult);

            // Act
            var actualResult = productStatusService.UpdateProductStatus(21, CustIdTest, ActorIdTest, IPTest, commands);

            // Assert
            Assert.False(errorResult.IsSuccess());
        }
    }
}
