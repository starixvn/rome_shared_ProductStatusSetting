//using System.Collections;
//using Fanex.ProductStatus.Setting.AppResources;
//using Fanex.ProductStatus.Setting.Models;
//using Xunit;

//namespace Fanex.ProductStatus.Setting.Tests.Models
//{
//    public class ResultTests
//    {
//        [Fact]
//        public void IsSuccess_ValidCode_ReturnsTrue()
//        {
//            // Arrange
//            var result = Result.CreateSuccessResult();

//            // Act
//            var actualResult = result.IsSuccess();

//            // Assert
//            Assert.True(actualResult);
//        }

//        [Fact]
//        public void IsSuccess_InvalidCode_ReturnsFalse()
//        {
//            // Arrange
//            var result = Result.CreateErrorResult(500, Resources.InternalServerError);

//            // Act
//            var actualResult = result.IsSuccess();

//            // Assert
//            Assert.False(actualResult);
//        }

//        [Fact]
//        public void CreateSuccessResult_Always_ReturnsTrue()
//        {
//            // Arrange
//            var expectedMessage = Resources.StatusUpdatedSuccessfully;

//            // Act
//            var actualResult = Result.CreateSuccessResult(expectedMessage);

//            // Assert
//            Assert.True(actualResult.IsSuccess() && actualResult.Message == expectedMessage);
//        }

//        [Fact]
//        public void CreateErrorResult_Always_ReturnsTrue()
//        {
//            // Arrange
//            var expectedMessage = Resources.InternalServerError;

//            // Act
//            var actualResult = Result.CreateErrorResult(500, expectedMessage);

//            // Assert
//            Assert.True(!actualResult.IsSuccess() && actualResult.Message == expectedMessage);
//        }

//        [Fact]
//        public void CreateResult_HashtableIsNull_Returns500()
//        {
//            // Arrange
//            var expectedCode = 500;

//            // Act
//            var actualResult = Result.CreateResult(null);

//            // Assert
//            Assert.True(!actualResult.IsSuccess() && actualResult.Code == expectedCode);
//        }

//        [Fact]
//        public void CreateResult_HashtableNotContainsErrCodeAndErrMsg_Returns500()
//        {
//            // Arrange
//            var expectedCode = 500;

//            // Act
//            var actualResult = Result.CreateResult(new Hashtable());

//            // Assert
//            Assert.True(!actualResult.IsSuccess() && actualResult.Code == expectedCode);
//        }

//        [Theory]
//        [InlineData(0, "Status updated successfully.")]
//        [InlineData(102, "")]
//        public void CreateResult_ValidHashtable_ReturnsCorrectResult(int code, string message)
//        {
//            // Arrange
//            var outputParams = new Hashtable
//            {
//                ["errcode"] = code,
//                ["errmsg"] = message,
//            };

//            // Act
//            var actualResult = Result.CreateResult(outputParams);

//            // Assert
//            Assert.True(actualResult.Code == code && actualResult.Message == message);
//        }
//    }
//}
