namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.Shared;
    using Setting.ViewModels;
    using Xunit;

    [Trait("ViewModels", "MultipleUpdateViewModel")]
    public class MultipleUpdateViewModelTests
    {
        private readonly IProductStatusActor productStatusActor;
        private readonly MultipleUpdateViewModel multipleUpdateViewModel;

        public MultipleUpdateViewModelTests()
        {
            productStatusActor = Substitute.For<IProductStatusActor>();
            multipleUpdateViewModel = new MultipleUpdateViewModel(productStatusActor);
        }

        [Fact]
        public void IsValid_CorrectData_ReturnsTrue()
        {
            // Arrange
            multipleUpdateViewModel.ProductId = ProductIdentity.NumberGame;
            multipleUpdateViewModel.CustIds = "1176092,14244423";
            multipleUpdateViewModel.CustNames = "member01,member012";

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

            productStatusActor.Id.Returns(8);

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

            productStatusActor.Id.Returns(8);

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

            productStatusActor.Id.Returns(8);

            // Act
            var actualResult = multipleUpdateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void IsValid_CustomersIsInvalidLength_ReturnsFalse()
        {
            // Arrange
            multipleUpdateViewModel.ProductId = ProductIdentity.NumberGame;
            multipleUpdateViewModel.CustIds = "1176092,14244423,20";
            multipleUpdateViewModel.CustNames = "member01,member012";
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

            productStatusActor.Id.Returns(8);

            // Act
            var actualResult = multipleUpdateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
