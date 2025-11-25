namespace Fanex.ProductStatus.Setting.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Fanex.ProductStatus.Setting.Configuration;
    using KellermanSoftware.CompareNetObjects;
    using Setting.Shared;
    using Setting.ViewModels;
    using Xunit;

    [Trait("ViewModels", "MultipleQueryViewModel")]
    public class MultipleQueryViewModelTests
    {
        [Fact]
        public void New_Always_AssignsCorrectProperties()
        {
            // Arrange & Act
            var custIds = "1,2,3";
            var custNames = "super1,super2,super3";
            var multipleQueryViewModel = new MultipleQueryViewModel(
                Enumerable.Empty<Product>(),
                new ProductStatusActorForTesting())
            {
                CustIds = custIds,
                CustNames = custNames
            };
            multipleQueryViewModel.CustRole = RoleIdentity.Member;

            // Assert
            Assert.True(
                custIds == multipleQueryViewModel.CustIds
                && custNames == multipleQueryViewModel.CustNames);
        }

        //[Fact]
        //public void Products_Super_ReturnsProductList()
        //{
        //    // Arrange
        //    var expectedResults = new List<Product>
        //    {
        //        new KeyValuePair<int, string>(3, "Number Game"),
        //        new KeyValuePair<int, string>(6, "Casino"),
        //        new KeyValuePair<int, string>(8, "Keno"),
        //        new KeyValuePair<int, string>(12, "Gold Deluxe"),
        //        new KeyValuePair<int, string>(21, "Allbets"),
        //        new KeyValuePair<int, string>(22, "Macau Games"),
        //        new KeyValuePair<int, string>(23, "NG2")
        //    };

        //    var multipleQueryViewModel = new MultipleQueryViewModel(expectedResults, new ProductStatusActorForTesting());
        //    multipleQueryViewModel.CustRole = RoleIdentity.Super;

        //    var compareLogic = new CompareLogic();

        //    // Act
        //    var actualResults = multipleQueryViewModel.Products;

        //    // Assert
        //    Assert.True(compareLogic.Compare(expectedResults, actualResults.ToList()).AreEqual);
        //}

        //[Fact]
        //public void Products_Member_ReturnsProductList()
        //{
        //    // Arrange
        //    var expectedResults = new List<KeyValuePair<int, string>>
        //    {
        //        new KeyValuePair<int, string>(3, "Number Game"),
        //        new KeyValuePair<int, string>(6, "Casino"),
        //        new KeyValuePair<int, string>(8, "Keno"),
        //        new KeyValuePair<int, string>(12, "Gold Deluxe"),
        //        new KeyValuePair<int, string>(15, "ColossusBets"),
        //        new KeyValuePair<int, string>(21, "Allbets"),
        //        new KeyValuePair<int, string>(22, "Macau Games")
        //    };

        //    var multipleQueryViewModel = new MultipleQueryViewModel(expectedResults, new ProductStatusActorForTesting())
        //    {
        //        ProductIds = "3,6,8,12,15,21,22"
        //    };
        //    multipleQueryViewModel.CustRole = RoleIdentity.Member;

        //    var compareLogic = new CompareLogic();

        //    // Act
        //    var actualResults = multipleQueryViewModel.Products;

        //    // Assert
        //    Assert.True(compareLogic.Compare(expectedResults, actualResults.ToList()).AreEqual);
        //}
    }
}