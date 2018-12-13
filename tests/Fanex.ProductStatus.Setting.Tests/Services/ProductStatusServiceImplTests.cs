namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Setting.Criteria;
    using Setting.Models;
    using Setting.Services;
    using Setting.Shared;
    using Xunit;

    [Trait("Services", "ProductStatusServiceImpl")]
    public class ProductStatusServiceImplTests
    {
        private const int CustIdTest = 1176083;
        private const int CustRoleTest = 3;
        private const int ActorIdTest = 8;
        private const string ActorNameTest = "sa";
        private const string IPTest = "127.0.0.1";
        private readonly static int[] CustIdsTest = new[] { 1176092, 14244423 };
        private readonly static string[] CustNamesTest = new[] { "member01", "member012" };

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
                    300)
            };
            var criteria = new SelectProductStatusCriteria(productId, CustIdTest);

            criteriaFactory
                .CreateSelectCriteria(productId, CustIdTest, 0)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria)
                .Returns(expectedResults);

            refDataService
                .GetProductName(productId)
                .Returns(productName);

            var compareLogic = new CompareLogic();

            // Act
            var actualData = productStatusService.GetProductStatus(productId, CustIdTest, 0);
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
                .CreateSelectCriteria(ProductIdentity.Keno, CustIdTest, 0)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria)
                .Returns(expectedResults);

            refDataService
                .GetBetTypeName(1501)
                .Returns("Australia");
            refDataService
                .GetBetTypeName(1503)
                .Returns("Slovakia");

            var compareLogic = new CompareLogic();

            // Act
            var actualResults = productStatusService.GetProductStatus(ProductIdentity.Keno, CustIdTest, 0);

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
                .CreateSelectCriteria(21, CustIdTest, 0)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria)
                .Returns(expectedResults);

            refDataService
                .GetBetTypeName(2401)
                .Returns("Baccarat, Roulette, Sic Bo, Dragon Tiger");
            refDataService
                .GetBetTypeName(2402)
                .Returns("VIP Baccarat");

            // Act
            var actualData = productStatusService.GetProductStatus(21, CustIdTest, 0);

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
                .CreateSelectCriteria(28, CustIdTest, 0)
                .Returns(criteria);

            productStatusDataAccess
                .GetProductStatus(criteria)
                .Returns(expectedResults);

            refDataService
                .GetProductName(28)
                .Returns("Lottery");

            var compareLogic = new CompareLogic();

            // Act
            var actualResults = productStatusService.GetProductStatus(28, CustIdTest, 0);

            // Assert
            Assert.True(compareLogic.Compare(expectedResults, actualResults.ToList()).AreEqual);
        }
    }
}
