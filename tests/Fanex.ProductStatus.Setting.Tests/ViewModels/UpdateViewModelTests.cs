namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.ViewModels;
    using Shared;
    using Xunit;

    public class UpdateViewModelTests
    {
        private readonly IProductStatusUser user;
        private readonly UpdateViewModel updateViewModel;

        public UpdateViewModelTests()
        {
            user = Substitute.For<IProductStatusUser>();
            updateViewModel = new UpdateViewModel(user);
        }

        [Fact]
        public void IsValid_CorrectData_ReturnsTrue()
        {
            // Arrange
            updateViewModel.ProductId = ProductIdentity.NumberGame;
            updateViewModel.CustId = 1176070;
            updateViewModel.ProductStatus = new ProductStatusCommand[]
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

            user.GetActorId().Returns(() => 8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void IsValid_InvalidProductId_ReturnsFalse(int productId)
        {
            // Arrange
            updateViewModel.ProductId = productId;
            updateViewModel.ProductStatus = new ProductStatusCommand[]
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

            user.GetActorId().Returns(() => 8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void IsValid_ProductStatusIsNull_ReturnsFalse()
        {
            // Arrange
            updateViewModel.ProductId = ProductIdentity.NumberGame;
            updateViewModel.ProductStatus = null;

            user.GetActorId().Returns(() => 8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void IsValid_ProductStatusIsEmpty_ReturnsFalse()
        {
            // Arrange
            updateViewModel.ProductId = ProductIdentity.NumberGame;
            updateViewModel.ProductStatus = new ProductStatusCommand[0];

            user.GetActorId().Returns(() => 8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void IsValid_InvalidActorId_ReturnsFalse(int actorId)
        {
            // Arrange
            updateViewModel.ProductId = ProductIdentity.NumberGame;
            updateViewModel.ProductStatus = new ProductStatusCommand[]
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

            user.GetActorId().Returns(() => actorId);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("::1")]
        [InlineData("127.0.0.1")]
        public void IP_Always_AssignsCorrectToIP(string ip)
        {
            // Arrange
            user.GetClientIP().Returns(() => ip);

            // Act
            var actualResult = updateViewModel.IP;

            // Assert
            Assert.Equal(ip, actualResult);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsTrue(int custId)
        {
            // Arrange
            updateViewModel.ProductId = ProductIdentity.NumberGame;
            updateViewModel.CustId = custId;
            updateViewModel.ProductStatus = new ProductStatusCommand[]
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

            user.GetActorId().Returns(() => 8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
