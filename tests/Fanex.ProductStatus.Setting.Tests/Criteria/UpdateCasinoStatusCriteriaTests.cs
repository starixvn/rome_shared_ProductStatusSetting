//using Fanex.ProductStatus.Setting.Models;
//using Xunit;

//namespace Fanex.ProductStatus.Setting.Tests.Criteria
//{
//    public class UpdateCasinoStatusCriteriaTests
//    {
//        private const int CustIdTest = 1176083;
//        private const int ActorIdTest = 8;
//        private const string IPTest = "127.0.0.1";
//        private readonly ProductStatusCommand command;

//        public UpdateCasinoStatusCriteriaTests()
//        {
//            command = ProductStatusCommand.DisableSuper();
//        }

//        [Fact]
//        public void GetSettingKey_Always_ReturnsValidSettingKey()
//        {
//            // Arrange
//            var criteria = new UpdateCasinoStatusCriteria(CustIdTest, SubAccIdTest, ActorIdTest, IPTest, LanguageTest, DisableTest);
//            var expectedSettingKey = "Acc_UpdCSStatus_Cust";

//            // Act
//            var actualSettingKey = criteria.GetSettingKey();

//            // Assert
//            Assert.Equal(expectedSettingKey, actualSettingKey);
//        }
//    }
//}
