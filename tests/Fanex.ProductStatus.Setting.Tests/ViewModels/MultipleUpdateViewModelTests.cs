namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.ViewModels;
    using Shared;
    using Xunit;

    public class MultipleUpdateViewModelTests
    {
        private readonly IProductStatusUser user;
        private readonly MultipleUpdateViewModel multipleUpdateViewModel;

        public MultipleUpdateViewModelTests()
        {
            user = Substitute.For<IProductStatusUser>();
            multipleUpdateViewModel = new MultipleUpdateViewModel(user);
        }

        [Fact]
        public void IsValid_CorrectData_ReturnsTrue()
        {
            // Arrange
            multipleUpdateViewModel.ProductId = ProductIdentity.NumberGame;
            multipleUpdateViewModel.Customers = new[]
            {
                new Customer(1176092, "member01"),
                new Customer(14244423, "member012"),
            };
            multipleUpdateViewModel.ProductStatus = new ProductStatusCommand[]
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
            var actualResult = multipleUpdateViewModel.IsValid();

            // Assert
            Assert.True(actualResult);
        }

        [Fact]
        public void IsValid_CustomersIsNull_ReturnsFalse()
        {
            // Arrange
            multipleUpdateViewModel.ProductId = ProductIdentity.NumberGame;
            multipleUpdateViewModel.Customers = null;
            multipleUpdateViewModel.ProductStatus = new ProductStatusCommand[]
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
            var actualResult = multipleUpdateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void IsValid_CustomersIsEmpty_ReturnsFalse()
        {
            // Arrange
            multipleUpdateViewModel.ProductId = ProductIdentity.NumberGame;
            multipleUpdateViewModel.Customers = new Customer[0];
            multipleUpdateViewModel.ProductStatus = new ProductStatusCommand[]
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
            var actualResult = multipleUpdateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
