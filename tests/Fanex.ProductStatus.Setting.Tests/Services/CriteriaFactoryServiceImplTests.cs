namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using System.Collections.Generic;
    using Setting.Criteria;
    using Setting.Models;
    using Setting.Services;
    using Setting.Shared;
    using Xunit;

    [Trait("Services", "CriteriaFactoryServiceImpl")]
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
        public void CreateSelectCriteria_ValidProductId_ReturnsSelectProductStatusCriteria(int productId)
        {
            // Arrange
            var expectedCriteria = new SelectProductStatusCriteria(productId, CustIdTest, 0);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(productId, CustIdTest, 0, 0);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateSelectCriteria_NG2_ReturnsSSelectNG2StatusCriteria()
        {
            // Arrange
            var expectedCriteria = new SelectNG2StatusCriteria(ProductIdentity.NG2, CustIdTest, 0);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(ProductIdentity.NG2, CustIdTest, 0, 0);

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
            var expectedCriteria = new SelectDCSProductStatusCriteria(productId, CustIdTest, 0);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(productId, CustIdTest, 0, 0);

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
    }
}
