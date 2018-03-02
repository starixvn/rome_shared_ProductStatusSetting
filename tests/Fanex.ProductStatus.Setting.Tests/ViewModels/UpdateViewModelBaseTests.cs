namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.ViewModels;
    using Shared;
    using Xunit;

    public class UpdateViewModelBaseTests
    {
        private readonly IProductStatusUser user;
        private readonly UpdateViewModelBase updateBase;

        public UpdateViewModelBaseTests()
        {
            user = Substitute.For<IProductStatusUser>();
            updateBase = new UpdateViewModelBase(user);
        }

        [Fact]
        public void IsValid_CorrectData_ReturnsTrue()
        {
            // Arrange
            updateBase.ProductId = ProductIdentity.NumberGame;
            updateBase.ProductStatus = new ProductStatusCommand[]
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
                }
            };

            user.ActorId.Returns(8);

            // Act
            var actualResult = updateBase.IsValid();

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void IsValid_InvalidProductId_ReturnsFalse(int productId)
        {
            // Arrange
            updateBase.ProductId = productId;
            updateBase.ProductStatus = new ProductStatusCommand[]
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
                }
            };

            user.ActorId.Returns(8);

            // Act
            var actualResult = updateBase.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void IsValid_ProductStatusIsNull_ReturnsFalse()
        {
            // Arrange
            updateBase.ProductId = ProductIdentity.NumberGame;
            updateBase.ProductStatus = null;

            user.ActorId.Returns(8);

            // Act
            var actualResult = updateBase.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void IsValid_ProductStatusIsEmpty_ReturnsFalse()
        {
            // Arrange
            updateBase.ProductId = ProductIdentity.NumberGame;
            updateBase.ProductStatus = new ProductStatusCommand[0];

            user.ActorId.Returns(8);

            // Act
            var actualResult = updateBase.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void IsValid_InvalidActorId_ReturnsFalse(int actorId)
        {
            // Arrange
            updateBase.ProductId = ProductIdentity.NumberGame;
            updateBase.ProductStatus = new ProductStatusCommand[]
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
                }
            };

            user.ActorId.Returns(actorId);

            // Act
            var actualResult = updateBase.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
