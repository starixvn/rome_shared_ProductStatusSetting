namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.Shared;
    using Setting.ViewModels;
    using Xunit;

    public class UpdateViewModelTests
    {
        private readonly IProductStatusUserService userService;
        private readonly UpdateViewModel updateViewModel;

        public UpdateViewModelTests()
        {
            userService = Substitute.For<IProductStatusUserService>();
            updateViewModel = new UpdateViewModel(userService);
        }

        [Fact]
        public void IsValid_CorrectData_ReturnsTrue()
        {
            // Arrange
            updateViewModel.ProductId = ProductIdentity.NumberGame;
            updateViewModel.CustId = 1176070;
            updateViewModel.CustRole = 3;
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

            userService.GetActorId().Returns(8);

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

            userService.GetActorId().Returns(8);

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

            userService.GetActorId().Returns(8);

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

            userService.GetActorId().Returns(8);

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

            userService.GetActorId().Returns(actorId);

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
            userService.GetClientIP().Returns(ip);

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

            userService.GetActorId().Returns(8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
