using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using NUnit.Framework;
using ZeroFormatter;

namespace Tests
{
    public class ZeroFormatterTests
    {
        public ZeroFormatterTests()
        { 
            ZeroFormatterInitializer.Register();
        }

        [Test]
        public void Test1()
        {
            //Arrange
           TestMessageClass1 testMessageClass1 = new TestMessageClass1
           {
               //12 байт на индекс
               //4 байта на свойство
               SomeNumber = 51
           };
            
            //Act
            byte[] serialized = ZeroFormatterSerializer.Serialize(testMessageClass1);
            
            //Assert
            Assert.AreEqual(16, serialized.Length);
        }
        
        [Test]
        public void Test2()
        {
            //Arrange
            TestMessageStruct1 testMessageClass1 = new TestMessageStruct1
            {
                //0 байт на индекс
                //4 байта на поле
                SomeNumber = 51
            };
            
            //Act
            byte[] serialized = ZeroFormatterSerializer.Serialize(testMessageClass1);
            
            //Assert
            Assert.AreEqual(4, serialized.Length);
        }
    }
}