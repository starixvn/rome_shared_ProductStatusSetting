namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Setting.Services;
    using Setting.Shared;
    using Setting.ViewModels;
    using Xunit;

    public class MultipleQueryViewModelTests
    {
        private readonly IProductStatusActor productStatusActor;
        private readonly MultipleQueryViewModel multipleQueryViewModel;

        public MultipleQueryViewModelTests()
        {
            productStatusActor = Substitute.For<IProductStatusActor>();
            multipleQueryViewModel = new MultipleQueryViewModel(productStatusActor);
        }

        [Fact]
        public void Products_Super_ReturnsProductList()
        {
            // Arrange
            var expectedResults = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(3, "Number Game"),
                new KeyValuePair<int, string>(6, "Casino"),
                new KeyValuePair<int, string>(8, "Keno"),
                new KeyValuePair<int, string>(12, "Gold Deluxe"),
                new KeyValuePair<int, string>(15, "ColossusBets"),
                new KeyValuePair<int, string>(21, "Allbets"),
                new KeyValuePair<int, string>(22, "Macau Games"),
                new KeyValuePair<int, string>(23, "NG2"),
                new KeyValuePair<int, string>(24, "Fishing World"),
                new KeyValuePair<int, string>(30, "Bullfighting")
            };

            multipleQueryViewModel.CustRole = RoleIdentity.Super;
            productStatusActor.GetProductList().Returns(expectedResults);

            var compareLogic = new CompareLogic();

            // Act
            var actualResults = multipleQueryViewModel.Products;

            // Assert
            Assert.True(compareLogic.Compare(expectedResults, actualResults.ToList()).AreEqual);
        }
    }
}
