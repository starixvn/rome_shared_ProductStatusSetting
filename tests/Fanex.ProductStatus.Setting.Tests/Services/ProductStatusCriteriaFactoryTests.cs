namespace Fanex.ProductStatus.Setting.Tests.Services
{
    using Setting.Criteria;
    using Setting.Services;
    using Shared;
    using Xunit;

    [Trait("Category", "ProductStatus")]
    public class ProductStatusCriteriaFactoryTests
    {
        private const int CustIdTest = 1176083;
        private readonly CriteriaFactoryService criteriaFactory;

        public ProductStatusCriteriaFactoryTests()
        {
            criteriaFactory = new CriteriaFactoryServiceImpl();
        }

        [Theory]
        [InlineData(ProductIdentity.Casino)]
        [InlineData(ProductIdentity.Keno)]
        [InlineData(ProductIdentity.ColossusBets)]
        public void CreateSelectCriteria_ValidProductId_ReturnsSelectProductStatusCriteria(int productId)
        {
            // Arrange
            var expectedCriteria = new SelectProductStatusCriteria(productId, CustIdTest);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(productId, CustIdTest);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateSelectCriteria_GoldDeluxe_ReturnsSelectGoldDeluxeStatusCriteria()
        {
            // Arrange
            var expectedCriteria = new SelectGoldDeluxeStatusCriteria(ProductIdentity.GoldDeluxe, CustIdTest);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(ProductIdentity.GoldDeluxe, CustIdTest);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Fact]
        public void CreateSelectCriteria_NG2_ReturnsSelectNG2StatusCriteria()
        {
            // Arrange
            var expectedCriteria = new SelectNG2StatusCriteria(ProductIdentity.NG2, CustIdTest);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(ProductIdentity.NG2, CustIdTest);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(21)]
        [InlineData(22)]
        [InlineData(24)]
        [InlineData(26)]
        [InlineData(28)]
        [InlineData(30)]
        [InlineData(int.MaxValue)]
        public void CreateSelectCriteria_ValidProductId_ReturnsSelectDCSProductStatusCriteria(int productId)
        {
            // Arrange
            var expectedCriteria = new SelectDCSProductStatusCriteria(productId, CustIdTest);

            // Act
            var actualCriteria = criteriaFactory.CreateSelectCriteria(productId, CustIdTest);

            // Assert
            Assert.Equal(expectedCriteria.GetSettingKey(), actualCriteria.GetSettingKey());
        }
    }
}
