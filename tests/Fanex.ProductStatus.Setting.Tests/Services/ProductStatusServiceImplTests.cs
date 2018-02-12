namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess;
    using NSubstitute;
    using Setting.Criteria;
    using Setting.Models;
    using Setting.Services;
    using Shared;
    using Xunit;

    public class ProductStatusServiceImplTests
    {
        private const int CustIdTest = 1176083;
        private const byte CustRoleIdTest = RoleIdentity.Super;

        private readonly ProductStatusDataAccess productStatusDataAccess;
        private readonly CriteriaFactoryService criteriaFactory;
        private readonly RefDataService refDataService;
        private readonly ProductStatusServiceImpl productStatusService;

        public ProductStatusServiceImplTests()
        {
            productStatusDataAccess = Substitute.For<ProductStatusDataAccess>();
            criteriaFactory = Substitute.For<CriteriaFactoryService>();
            refDataService = Substitute.For<RefDataService>();

            productStatusService = new ProductStatusServiceImpl(productStatusDataAccess, criteriaFactory, refDataService);
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
                    300,
                    CustRoleIdTest)
            };
            var criteria = new SelectProductStatusCriteria(productId, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(productId, CustIdTest)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria, CustRoleIdTest)
                .Returns(expectedResults);

            refDataService
                .GetProductName(productId)
                .Returns(productName);

            // Act
            var actualData = productStatusService.GetProductStatus(productId, CustIdTest, CustRoleIdTest);
            var actualResult = actualData.First();

            // Assert
            Assert.True(
                expectedResults[0].ProductId == actualResult.ProductId
                && expectedResults[0].Name == actualResult.Name
                && expectedResults[0].SportType == actualResult.SportType
                && expectedResults[0].BetType == actualResult.BetType
                && expectedResults[0].IsDisabled == actualResult.IsDisabled
                && expectedResults[0].IsDisabledUpline == actualResult.IsDisabledUpline
                && expectedResults[0].Master.TotalDisabled == actualResult.Master.TotalDisabled
                && expectedResults[0].Master.TotalEnabled == actualResult.Master.TotalEnabled
                && expectedResults[0].Agent.TotalDisabled == actualResult.Agent.TotalDisabled
                && expectedResults[0].Agent.TotalEnabled == actualResult.Agent.TotalEnabled
                && expectedResults[0].Member.TotalDisabled == actualResult.Member.TotalDisabled
                && expectedResults[0].Member.TotalEnabled == actualResult.Member.TotalEnabled);
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
                    300,
                    CustRoleIdTest),
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
                    300,
                    CustRoleIdTest)
            };
            var criteria = new SelectProductStatusCriteria(ProductIdentity.Keno, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(ProductIdentity.Keno, CustIdTest)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria, CustRoleIdTest)
                .Returns(expectedResults);

            refDataService
                .GetBetTypeName(1501)
                .Returns("Australia");
            refDataService
                .GetBetTypeName(1503)
                .Returns("Slovakia");

            // Act
            var actualData = productStatusService.GetProductStatus(ProductIdentity.Keno, CustIdTest, CustRoleIdTest);

            // Assert
            Assert.True(expectedResults.Count == actualData.Count());
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
                    300,
                    CustRoleIdTest),
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
                    300,
                    CustRoleIdTest)
            };
            var criteria = new SelectDCSProductStatusCriteria(21, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(21, CustIdTest)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria, CustRoleIdTest)
                .Returns(expectedResults);

            refDataService
                .GetBetTypeName(2401)
                .Returns("Baccarat, Roulette, Sic Bo, Dragon Tiger");
            refDataService
                .GetBetTypeName(2402)
                .Returns("VIP Baccarat");

            // Act
            var actualData = productStatusService.GetProductStatus(21, CustIdTest, CustRoleIdTest);

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
                    300,
                    CustRoleIdTest)
            };
            var criteria = new SelectDCSProductStatusCriteria(28, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(28, CustIdTest)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria, CustRoleIdTest)
                .Returns(expectedResults);

            refDataService
                .GetProductName(28)
                .Returns("Lottery");

            // Act
            var actualData = productStatusService.GetProductStatus(28, CustIdTest, CustRoleIdTest);
            var actualResult = actualData.First();

            // Assert
            Assert.True(
                expectedResults[0].ProductId == actualResult.ProductId
                && expectedResults[0].Name == actualResult.Name
                && expectedResults[0].SportType == actualResult.SportType
                && expectedResults[0].BetType == actualResult.BetType
                && expectedResults[0].IsDisabled == actualResult.IsDisabled
                && expectedResults[0].IsDisabledUpline == actualResult.IsDisabledUpline
                && expectedResults[0].Master.TotalDisabled == actualResult.Master.TotalDisabled
                && expectedResults[0].Master.TotalEnabled == actualResult.Master.TotalEnabled
                && expectedResults[0].Agent.TotalDisabled == actualResult.Agent.TotalDisabled
                && expectedResults[0].Agent.TotalEnabled == actualResult.Agent.TotalEnabled
                && expectedResults[0].Member.TotalDisabled == actualResult.Member.TotalDisabled
                && expectedResults[0].Member.TotalEnabled == actualResult.Member.TotalEnabled);
        }
    }
}
