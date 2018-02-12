namespace Fanex.ProductStatus.Setting.Tests.Criteria
{
    using System;
    using Setting.Models;
    using Xunit;

    public class UpdateProductStatusCriteriaBaseTests
    {
        private const int CustIdTest = 1176083;
        private const int ActorIdTest = 8;
        private const string IPTest = "127.0.0.1";
        private static readonly ProductStatusCommand CommandTest = new ProductStatusCommand
        {
            SportType = 211,
            BetType = 2401,
            Disable = false,
            UpdateAllMasters = true,
            DisableAllMasters = false,
            UpdateAllAgents = true,
            DisableAllAgents = false,
            UpdateAllMembers = true,
            DisableAllMembers = false
        };

        [Fact]
        public void GetSettingKey_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var updateCriteriaBase = new UpdateProductStatusCriteriaBaseForTesting(CustIdTest, ActorIdTest, IPTest, CommandTest);

            // Act& Assert
            Assert.Throws<NotImplementedException>(() => updateCriteriaBase.GetSettingKey());
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var updateCriteriaBase = new UpdateProductStatusCriteriaBaseForTesting(CustIdTest, ActorIdTest, IPTest, CommandTest);
            var expectedResult = true;

            // Act
            var actualResult = updateCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(0, false)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_CustId_ReturnsExpectedResult(int custId, bool expectedResult)
        {
            // Arrange
            var updateCriteriaBase = new UpdateProductStatusCriteriaBaseForTesting(custId, ActorIdTest, IPTest, CommandTest);

            // Act
            var actualResult = updateCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(0, false)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_ActorId_ReturnsExpectedResult(int actorId, bool expectedResult)
        {
            // Arrange
            var updateCriteriaBase = new UpdateProductStatusCriteriaBaseForTesting(CustIdTest, actorId, IPTest, CommandTest);

            // Act
            var actualResult = updateCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
