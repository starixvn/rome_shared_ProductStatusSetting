namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Setting.Criteria;
    using Setting.Models;
    using Setting.Services;
    using Setting.Shared;
    using Xunit;

    public class CriteriaFactoryServiceImplTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";

        private readonly CriteriaFactoryServiceImpl criteriaFactory;

        public CriteriaFactoryServiceImplTests()
        {
            criteriaFactory = new CriteriaFactoryServiceImpl();
        }

        [Theory]
        [InlineData(ProductIdentity.NumberGame)]
        [InlineData(ProductIdentity.Casino)]
        [InlineData(ProductIdentity.Keno)]
        [InlineData(ProductIdentity.GoldDeluxe)]
        [InlineData(ProductIdentity.ColossusBets)]
        public void CreateSelectCriteria_ValidProductId_ReturnsSelectProductStatusCriteria(int productId)
        {
            // Arrange
            var expectedCriteria = new SelectProductStatusCriteria(productId, CustIdTest);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(productId, CustIdTest);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateSelectCriteria_NG2_ReturnsSSelectNG2StatusCriteria()
        {
            // Arrange
            var expectedCriteria = new SelectNG2StatusCriteria(ProductIdentity.NG2, CustIdTest);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(ProductIdentity.NG2, CustIdTest);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Theory]
        [InlineData(21)]
        [InlineData(30)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        public void CreateSelectCriteria_DCSProducts_ReturnsSelectDCSProductStatusCriteria(int productId)
        {
            // Arrange
            var expectedCriteria = new SelectDCSProductStatusCriteria(productId, CustIdTest);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(productId, CustIdTest);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateUpdateCriteria_NumberGame_ReturnsUpdateNumberGameStatusCriteria()
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 161,
                BetType = 81,
                Disable = false,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = false,
                UpdateAllMembers = true,
                DisableAllMembers = false
            };
            var expectedCriteria = new UpdateNumberGameStatusCriteria(CustIdTest, ActorIdTest, IPTest, command);

            // Act
            var actualCriteria = criteriaFactory.CreateUpdateCriteria(
                ProductIdentity.NumberGame,
                CustIdTest,
                ActorIdTest,
                IPTest,
                new List<ProductStatusCommand> { command });

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateUpdateCriteria_Casino_ReturnsUpdateCasinoStatusCriteria()
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 208,
                BetType = 2101,
                Disable = false,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = false,
                UpdateAllMembers = true,
                DisableAllMembers = false
            };
            var expectedCriteria = new UpdateCasinoStatusCriteria(CustIdTest, ActorIdTest, IPTest, command);

            // Act
            var actualCriteria = criteriaFactory.CreateUpdateCriteria(
                ProductIdentity.Casino,
                CustIdTest,
                ActorIdTest,
                IPTest,
                new List<ProductStatusCommand> { command });

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateUpdateCriteria_GoldDeluxe_ReturnsUpdateGoldDeluxeStatusCriteria()
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 255,
                BetType = 1700,
                Disable = false,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = false,
                UpdateAllMembers = true,
                DisableAllMembers = false
            };
            var expectedCriteria = new UpdateGoldDeluxeStatusCriteria(CustIdTest, ActorIdTest, IPTest, command);

            // Act
            var actualCriteria = criteriaFactory.CreateUpdateCriteria(
                ProductIdentity.GoldDeluxe,
                CustIdTest,
                ActorIdTest,
                IPTest,
                new List<ProductStatusCommand> { command });

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateUpdateCriteria_ColossusBets_ReturnsUpdateColossusBetsStatusCriteria()
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 204,
                BetType = 18000,
                Disable = false,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = false,
                UpdateAllMembers = true,
                DisableAllMembers = false
            };
            var expectedCriteria = new UpdateColossusBetsStatusCriteria(CustIdTest, ActorIdTest, IPTest, command);

            // Act
            var actualCriteria = criteriaFactory.CreateUpdateCriteria(
                ProductIdentity.ColossusBets,
                CustIdTest,
                ActorIdTest,
                IPTest,
                new List<ProductStatusCommand> { command });

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateUpdateCriteria_NG2_ReturnsUpdateNG2StatusCriteria()
        {
            // Arrange
            var commands = new[]
            {
                new ProductStatusCommand
                {
                    SportType = 213,
                    BetType = 2501,
                    Disable = false,
                    UpdateAllMasters = true,
                    DisableAllMasters = false,
                    UpdateAllAgents = true,
                    DisableAllAgents = false,
                    UpdateAllMembers = true,
                    DisableAllMembers = false
                }
            };
            var expectedCriteria = new UpdateNG2StatusCriteria(CustIdTest, ActorIdTest, IPTest, commands);

            // Act
            var actualCriteria = criteriaFactory.CreateUpdateCriteria(
                ProductIdentity.NG2,
                CustIdTest,
                ActorIdTest,
                IPTest,
                commands);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Theory]
        [InlineData(21)]
        [InlineData(30)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        public void CreateUpdateCriteria_DCSProducts_ReturnsUpdateDCSProductStatusCriteria(int productId)
        {
            // Arrange
            var commands = new[]
            {
                new ProductStatusCommand
                {
                    SportType = 211,
                    BetType = 2401,
                    Disable = false,
                    UpdateAllMasters = true,
                    DisableAllMasters = false,
                    UpdateAllAgents = true,
                    DisableAllAgents = false,
                    UpdateAllMembers = true,
                    DisableAllMembers = false
                },
                new ProductStatusCommand
                {
                    SportType = 211,
                    BetType = 2402,
                    Disable = false,
                    UpdateAllMasters = true,
                    DisableAllMasters = false,
                    UpdateAllAgents = true,
                    DisableAllAgents = false,
                    UpdateAllMembers = true,
                    DisableAllMembers = false
                }
            };
            var expectedCriteria = new UpdateDCSProductStatusCriteria(CustIdTest, ActorIdTest, IPTest, commands);

            // Act
            var actualCriteria = criteriaFactory.CreateUpdateCriteria(
                productId,
                CustIdTest,
                ActorIdTest,
                IPTest,
                commands);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateUpdateKenoStatusCriteria_Always_ReturnsCorrectCriteria()
        {
            // Arrange
            var command = new ProductStatusCommand
            {
                SportType = 202,
                BetType = 1501,
                Disable = false,
                UpdateAllMasters = true,
                DisableAllMasters = false,
                UpdateAllAgents = true,
                DisableAllAgents = false,
                UpdateAllMembers = true,
                DisableAllMembers = false
            };
            var expectedCriteria = new UpdateKenoStatusCriteria(CustIdTest, ActorIdTest, IPTest, command);

            // Act
            var actualCriteria = criteriaFactory.CreateUpdateKenoStatusCriteria(
                CustIdTest,
                ActorIdTest,
                IPTest,
                new List<ProductStatusCommand> { command });

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.First().GetSettingKey());
        }
    }
}
