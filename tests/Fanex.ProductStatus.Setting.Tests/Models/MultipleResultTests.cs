namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using System.Collections;
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Setting.Shared;
    using Xunit;

    [Trait("Models", "MultipleResult")]
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
            var actualResult = new MultipleResult(new Result(0, null), custId, custName);

            // Assert
            Assert.True(
                custId == actualResult.CustId
                && custName == actualResult.CustName);
        }

        [Fact]
        public void New_HashtableIsNull_Returns500()
        {
            // Arrange
            var expectedCode = 500;

            // Act
            var actualResult = new MultipleResult(new Result(outputParams:null), 1176070, "SuperTest1");

            // Assert
            Assert.True(
                !actualResult.IsSuccess()
                && expectedCode == actualResult.Code);
        }

        [Fact]
        public void New_HashtableNotContainsErrCodeAndErrMsg_Returns500()
        {
            // Arrange
            var expectedCode = 500;

            // Act
            var actualResult = new MultipleResult(new Result(new Hashtable()), 1176070, "SuperTest1");

            // Assert
            Assert.True(
                !actualResult.IsSuccess()
                && expectedCode == actualResult.Code);
        }

        [Theory]
        [InlineData(0, "Status updated successfully.")]
        [InlineData(400, "You cannot submit the action due to a validation error.")]
        [InlineData(400, "<You cannot submit the action due to a validation error.")]
        public void ParseDCSProductMessage_ErrMsgIsNotXmlFormat_ReturnsCorrectResult(int code, string message)
        {
            // Arrange
            var outputParams = new Hashtable
            {
                ["errcode"] = code,
                ["errmsg"] = message,
            };

            // Act
            var actual = new MultipleResult(new Result(outputParams), 1176070, "SuperTest1");
            var actualResult = actual.ParseDCSProductMessage(ProductIdentity.NumberGame);

            // Assert
            Assert.True(
                code == actual.Code
                && message == actualResult);
        }

        [Fact]
        public void New_ErrMsgIsXmlFormat_ReturnsCorrectResult()
        {
            // Arrange
            var outputParams = new Hashtable
            {
                ["errcode"] = 102,
                ["errmsg"] = "<Root MsgID=\"60035\" Msg=\"You are not allowed to disable for downlines.\"><Row SportType=\"220\" BetType=\"1021\"/></Root>",
            };
            RefDataService refDataService = Substitute.For<RefDataService>();
            refDataService.GetBetTypeName(Arg.Any<int>()).Returns("VIP Baccarat");

            // Act
            var result = new MultipleResult(new Result(outputParams), 1176070, "SuperTest1");
            var actualResult = result.ParseDCSProductMessage(ProductIdentity.LiveCasino);

            // Assert
            Assert.True(
                !result.IsSuccess()
               && "You are not allowed to disable downline's product." == actualResult);
        }
    }
}
