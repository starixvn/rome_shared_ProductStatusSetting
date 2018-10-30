namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Setting.Models;
    using Setting.Shared;
    using Setting.ViewModels;
    using Xunit;

    [Trait("ViewModels", "SimpleQueryViewModel")]
    public class SimpleQueryViewModelTests
    {
        private const int ProductIdTest = ProductIdentity.NumberGame;
        private const int CustIdTest = 1176083;
        private const string CustNameTest = "supertest1";
        private const byte CustRoleTest = RoleIdentity.Super;

        [Fact]
        public void New_Always_AssignsCorrectProperties()
        {
            // Arrange & Act
            var viewModel = new SimpleQueryViewModel
            {
                ProductId = ProductIdTest,
                CustId = CustIdTest,
                CustName = CustNameTest,
                CustRole = CustRoleTest
            };

            // Assert
            Assert.True(
                ProductIdTest == viewModel.ProductId
                && CustIdTest == viewModel.CustId
                && CustNameTest == viewModel.CustName
                && CustRoleTest == viewModel.CustRole);
        }

        [Fact]
        public void SetSimpleProductStatusViewModel_InputIsEmpty_ReturnsEmpty()
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel
            {
                ProductId = ProductIdTest,
                CustId = CustIdTest,
                CustName = CustNameTest,
                CustRole = CustRoleTest
            };

            // Act
            viewModel.SetSimpleProductStatusViewModel(Enumerable.Empty<ProductStatusModel>());

            // Assert
            Assert.True(!viewModel.ProductStatus.Any()
                && !viewModel.HasData);
        }

        [Fact]
        public void SetSimpleProductStatusViewModel_InputHasData_ReturnsCorrectProductStatusViewModel()
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel
            {
                ProductId = ProductIdTest,
                CustId = CustIdTest,
                CustName = CustNameTest,
                CustRole = CustRoleTest
            };

            var productStatus = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    sportType: 5,
                    betType: 12,
                    isDisabled: true,
                    isDisabledUpline: false,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 78,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 37,
                    totalDisabledMembers: 36,
                    totalEnabledMembers: 46),
                new ProductStatusModel(
                    sportType: 5,
                    betType: 13,
                    isDisabled: false,
                    isDisabledUpline: false,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 0,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 0,
                    totalDisabledMembers: 36,
                    totalEnabledMembers: 0),
                  new ProductStatusModel(
                    sportType: 5,
                    betType: 14,
                    isDisabled: false,
                    isDisabledUpline: false,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 78,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 37,
                    totalDisabledMembers: 0,
                    totalEnabledMembers: 0)
            };

            // Act
            viewModel.SetSimpleProductStatusViewModel(productStatus);

            // Assert
            Assert.True(viewModel.ProductStatus.Any()
                && viewModel.HasData);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetSimpleProductStatusViewModel_IsDisabledUpline_ReturnsCorrectAllowUpdate(bool disabledUpline)
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel
            {
                ProductId = ProductIdTest,
                CustId = CustIdTest,
                CustName = CustNameTest,
                CustRole = CustRoleTest
            };

            var productStatus = new List<ProductStatusModel>
            {
                new ProductStatusModel(
                    sportType: 5,
                    betType: 12,
                    isDisabled: false,
                    isDisabledUpline: disabledUpline,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 78,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 37,
                    totalDisabledMembers: 36,
                    totalEnabledMembers: 46),
                new ProductStatusModel(
                    sportType: 5,
                    betType: 13,
                    isDisabled: false,
                    isDisabledUpline: disabledUpline,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 0,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 0,
                    totalDisabledMembers: 36,
                    totalEnabledMembers: 0),
                  new ProductStatusModel(
                    sportType: 5,
                    betType: 14,
                    isDisabled: false,
                    isDisabledUpline: disabledUpline,
                    totalDisabledMasters: 12,
                    totalEnabledMasters: 78,
                    totalDisabledAgents: 33,
                    totalEnabledAgents: 37,
                    totalDisabledMembers: 0,
                    totalEnabledMembers: 0)
            };

            // Act
            viewModel.SetSimpleProductStatusViewModel(productStatus);

            // Assert
            Assert.True(viewModel.AllowUpdate == !disabledUpline);
        }

        [Theory]
        [InlineData(RoleIdentity.Super, "Super")]
        [InlineData(RoleIdentity.Master, "Master")]
        [InlineData(RoleIdentity.Agent, "Agent")]
        [InlineData(RoleIdentity.Member, "Member")]
        [InlineData(10, null)]
        public void GetRoleName_Always_ReturnsCorrectName(byte custRole, string roleName)
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel
            {
                CustRole = custRole
            };

            // Act
            var actualResult = viewModel.GetRoleName();

            // Assert
            Assert.Equal(roleName, actualResult);
        }

        [Fact]
        public void IsValid_CorrectData_ReturnsTrue()
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel
            {
                ProductId = ProductIdTest,
                CustId = CustIdTest,
                CustRole = CustRoleTest
            };

            // Act
            var actualResult = viewModel.IsValid();

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void IsValid_InvalidProductId_ReturnsFalse(int productId)
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel
            {
                ProductId = productId,
                CustId = CustIdTest,
                CustRole = CustRoleTest
            };

            // Act
            var actualResult = viewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel
            {
                ProductId = ProductIdTest,
                CustId = custId,
                CustRole = CustRoleTest
            };

            // Act
            var actualResult = viewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(5)]
        [InlineData(byte.MaxValue)]
        public void IsValid_InvalidCustRole_ReturnsFalse(byte custRole)
        {
            // Arrange
            var viewModel = new SimpleQueryViewModel
            {
                ProductId = ProductIdTest,
                CustId = CustIdTest,
                CustRole = custRole
            };

            // Act
            var actualResult = viewModel.IsValid();

            // Assert
            Assert.False(actualResult);
        }
    }
}
