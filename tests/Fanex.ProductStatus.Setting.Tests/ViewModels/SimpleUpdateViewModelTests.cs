namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.Shared;
    using Setting.ViewModels;
    using Xunit;

    [Trait("ViewModels", "SimpleUpdateViewModel")]
    public class SimpleUpdateViewModelTests
    {
        private readonly IProductStatusActor productStatusActor;
        private readonly SimpleUpdateViewModel updateViewModel;

        public SimpleUpdateViewModelTests()
        {
            productStatusActor = Substitute.For<IProductStatusActor>();
            updateViewModel = new SimpleUpdateViewModel(productStatusActor);
        }

        [Fact]
        public void New_Always_ReturnsCorrectActorId()
        {
            // Arrange
            productStatusActor.Id.Returns(8);
            productStatusActor.Name.Returns("sa");

            // Act & Assert
            Assert.True(updateViewModel.ActorId == 8
                && updateViewModel.ActorName == "sa");
        }

        [Fact]
        public void IsValid_CorrectData_ReturnsTrue()
        {
            // Arrange
            updateViewModel.ProductId = ProductIdentity.NumberGame;
            updateViewModel.CustId = 1176070;
            updateViewModel.CustRole = 3;
            updateViewModel.ProductStatus = new SimpleProductStatusCommand[]
            {
                new SimpleProductStatusCommand
                {
                    SportType = 211,
                    BetType = 2401,
                    Disable = true,
                    DisableDownlines = true
                }
            };

            productStatusActor.Id.Returns(8);

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
            updateViewModel.ProductStatus = new SimpleProductStatusCommand[]
            {
                new SimpleProductStatusCommand
                {
                    SportType = 211,
                    BetType = 2401,
                    Disable = true,
                    DisableDownlines = true
                }
            };

            productStatusActor.Id.Returns(8);

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

            productStatusActor.Id.Returns(8);

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
            updateViewModel.ProductStatus = new SimpleProductStatusCommand[0];

            productStatusActor.Id.Returns(8);

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
            updateViewModel.ProductStatus = new SimpleProductStatusCommand[]
            {
                new SimpleProductStatusCommand
                {
                    SportType = 211,
                    BetType = 2401,
                    Disable = true,
                    DisableDownlines = true
                }
            };

            productStatusActor.Id.Returns(actorId);

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
            productStatusActor.ClientIP.Returns(ip);

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
            updateViewModel.ProductStatus = new SimpleProductStatusCommand[]
            {
                new SimpleProductStatusCommand
                {
                    SportType = 211,
                    BetType = 2401,
                    Disable = true,
                    DisableDownlines = true
                }
            };

            productStatusActor.Id.Returns(8);

            // Act
            var actualResult = updateViewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
