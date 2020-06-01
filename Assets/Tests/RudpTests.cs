using System.Threading.Tasks;
using NUnit.Framework;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Tests
{
    public class RudpTests
    {
        public RudpTests()
        {
            //Нужно, чтобы сообщения сериализовались
            ZeroFormatterInitializer.Register();
        }

        [Test]
        public void Test1()
        {
            //Arrange
            ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();
            int matchId = 5;
            int playerId = 5;
            
            //Act
            Task.Run(() =>
            {
                for (uint i = 0; i < 10000; i++)
                {
                    byte[] data = new byte[100];
                    byteArrayRudpStorage.AddReliableMessage(matchId, playerId, i, data);
                }
            });
            
            Task.Run(() =>
            {
                for (uint i = 0; i < 10000; i++)
                {
                    byte[] data = new byte[100];
                    byteArrayRudpStorage.TryRemoveMessage(i);
                }
            });
           
            for (int i = 0; i < 10000; i++)
            {
                byte[][] messages = byteArrayRudpStorage.GetMessages(matchId, playerId);
            }
          
        }
        
        [Test]
        public void Test2()
        {
            //Arrange
            //Act
            //Assert
        }
        
        [Test]
        public void Test3()
        {
            //Arrange
            //Act
            //Assert
        }
        [Test]
        public void Test4()
        {
            //Arrange
            //Act
            //Assert
        }
        [Test]
        public void Test5()
        {
            //Arrange
            //Act
            //Assert
        }
        [Test]
        public void Test6()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}