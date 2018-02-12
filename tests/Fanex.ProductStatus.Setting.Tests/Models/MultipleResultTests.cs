namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using Setting.Models;
    using Xunit;

    public class MultipleResultTests
    {
        [Theory]
        [InlineData(1176070, "SuperTest1")]
        [InlineData(1176083, "master1")]
        [InlineData(0, null)]
        [InlineData(0, "")]
        public void New_Always_AssignCorrectProperties(int custId, string custName)
        {
            // Arrange && Act
            var actualResult = new MultipleResult(0, null, custId, custName);

            // Assert
            Assert.True(
                custId == actualResult.CustId
                && custName == actualResult.CustName);
        }
    }
}
