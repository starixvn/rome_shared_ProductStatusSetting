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

            user.ActorId.Returns(8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.True(actualResult);
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

            user.ActorId.Returns(8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
