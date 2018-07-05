namespace Fanex.ProductStatus.Setting.Tests.Models
{
    using System.Collections;
    using AppResources;
    using NSubstitute;
    using Setting.Models;
    using Setting.Services;
    using Xunit;

    [Trait("Models", "Result")]
    public class ResultTests
    {
        private readonly RefDataServiceProvider refDataServiceProvider;

        public ResultTests()
        {
            refDataServiceProvider = Substitute.For<RefDataServiceProvider>();
            refDataServiceProvider.GetBetTypeName(Arg.Any<int>()).Returns("VIP Baccarat");
        }

        [Theory]
        [InlineData(0, "Status updated successfully.")]
        [InlineData(500, "Internal server error.")]
        [InlineData(400, "You cannot submit the action due to a validation error.")]
        [InlineData(403, "You are not authorized on this function.")]
        public void New_Always_AssignCorrectProperties(int code, string message)
        {
            // Arrange && Act
            var actualResult = new Result(code, message);

            // Assert
            Assert.True(
                code == actualResult.Code
                && message == actualResult.Message);
        }

        [Fact]
        public void New_HashtableIsNull_Returns500()
        {
            // Arrange
            var expectedCode = 500;

            // Act
            var actualResult = new Result(outputParams: null);

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
            var actualResult = new Result(new Hashtable());

            // Assert
            Assert.True(
                !actualResult.IsSuccess()
                && expectedCode == actualResult.Code);
        }

        [Theory]
        [InlineData(0, "Status updated successfully.")]
        [InlineData(400, "You cannot submit the action due to a validation error.")]
        [InlineData(400, "<You cannot submit the action due to a validation error.")]
        public void New_ErrMsgIsNotXmlFormat_ReturnsCorrectResult(int code, string message)
        {
            // Arrange
            var outputParams = new Hashtable
            {
                ["errcode"] = code,
                ["errmsg"] = message,
            };

            // Act
            var actualResult = new Result(outputParams);

            // Assert
            Assert.True(
                code == actualResult.Code
                && message == actualResult.Message);
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

            // Act
            var actualResult = new Result(outputParams, refDataServiceProvider);

            // Assert
            Assert.True(
                !actualResult.IsSuccess()
                && "You are not allowed to disable downline's product.<br/>VIP Baccarat" == actualResult.Message);
        }

        [Fact]
        public void IsSuccess_ValidCode_ReturnsTrue()
        {
            // Arrange && Act
            var actualResult = new Result(0, Resources.StatusUpdatedSuccessfully);

            // Assert
            Assert.True(actualResult.IsSuccess());
        }

        [Fact]
        public void IsSuccess_InvalidCode_ReturnsFalse()
        {
            // Arrange && Act
            var actualResult = new Result(500, Resources.InternalServerError);

            // Assert
            Assert.False(actualResult.IsSuccess());
        }
    }
}
