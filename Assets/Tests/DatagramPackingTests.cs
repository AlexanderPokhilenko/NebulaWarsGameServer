// using System.Collections.Generic;
// using System.Net;
// using NUnit.Framework;
// using ZeroFormatter;
//
// namespace Tests
// {
//     public class DatagramPackingTests
//     {
//         public DatagramPackingTests()
//         {
//             //Нужно, чтобы сообщения сериализовались в массивы байт
//             ZeroFormatterInitializer.Register();
//         }
//         
//         [Test]
//         public void NullMessagesList()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             
//             //Act + Assert
//             Assert.That(() =>
//                 {
//                     shittyDatagramPacker.Send(new IPEndPoint(0, 0), null);
//                 }
//                 , Throws.Exception);
//         }
//         
//         [Test]
//         public void NegativeMtu()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = -1;
//             
//             //Act + Assert
//             Assert.That(() =>
//                 {
//                     ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//                 }
//                 , Throws.Exception);
//         }
//         
//         [Test]
//         public void ZeroMtu()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 0;
//             
//             //Act + Assert
//             Assert.That(() =>
//                 {
//                     ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//                 }
//                 , Throws.Exception);
//         }
//         
//         [Test]
//         public void EmptyMessagesList()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>();
//
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             Assert.AreEqual(0, numbersOfPackages);
//         }
//         
//         
//         [Test]
//         public void OnePacket()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>
//             {
//                 new byte[100]
//             };
//
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             Assert.AreEqual(1, numbersOfPackages);
//         }
//         
//         [Test]
//         public void TenPacketsBy100Bytes()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>();
//             for (int i = 0; i < 10; i++)
//             {
//                 messages.Add(new byte[100]);
//             }
//             
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             Assert.AreEqual(1, numbersOfPackages);
//         }
//         
//         [Test]
//         public void ElevenPacketsBy100Bytes()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>();
//             for (int i = 0; i < 11; i++)
//             {
//                 messages.Add(new byte[100]);
//             }
//             
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             Assert.AreEqual(1, numbersOfPackages);
//         }
//         
//         
//         [Test]
//         public void ThirteenPacketsBy100Bytes()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>();
//             for (int i = 0; i < 13; i++)
//             {
//                 messages.Add(new byte[100]);
//             }
//             
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             Assert.AreEqual(2, numbersOfPackages);
//         }
//         
//          [Test]
//          public void MessageSizeIs1MoreThanMtu()
//          {
//              //Arrange
//              MockUdpSender mockUdpSender = new MockUdpSender();
//              int mtu = 1200;
//              ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//              List<byte[]> messages = new List<byte[]>();
//              for (int i = 0; i < 12; i++)
//              {
//                  messages.Add(new byte[100]);
//              }
//              messages.Add(new byte[1]);
//              
//              //Act
//              shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//              
//              //Assert
//              int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//              Assert.AreEqual(2, numbersOfPackages);
//          }
//         
//         
//            
//          [Test]
//          public void TooLongMessage()
//          {
//              //Arrange
//              MockUdpSender mockUdpSender = new MockUdpSender();
//              int mtu = 1200;
//              ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//              List<byte[]> messages = new List<byte[]>(new[]
//              {
//                  new byte[mtu+1]
//              });
//         
//              //Act + Assert
//              Assert.That(()=>
//              {
//                  shittyDatagramPacker.Send(new IPEndPoint(0, 0), messages);
//              }, Throws.Exception);
//          }
//         
//         [Test]
//         public void ThreeDatagrams()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>();
//             for (int i = 0; i < 33; i++)
//             {
//                 messages.Add(new byte[100]);
//             }
//
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             Assert.AreEqual(3, numbersOfPackages);
//         }
//       
//         [Test]
//         public void FourDatagrams()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>();
//             for (int i = 0; i < 34; i++)
//             {
//                 messages.Add(new byte[100]);
//             }
//
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             Assert.AreEqual(4, numbersOfPackages);
//         }
//
//         
//         [Test]
//         public void Suka()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>();
//             for (int i = 1; i <= 11; i++)
//             {
//                 messages.Add(new byte[100]);
//             }
//
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             Assert.AreEqual(1, numbersOfPackages);
//         }
//
//
//         [Test]
//         public void ALotMessages()
//         {
//             //Arrange
//             MockUdpSender mockUdpSender = new MockUdpSender();
//             int mtu = 1200;
//             ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
//             List<byte[]> messages = new List<byte[]>(new[]
//             {
//                 new byte[1516],
//                 
//                 new byte[1516],
//                 
//                 new byte[500],
//                 new byte[500],
//                 
//                 new byte[500],
//                 new byte[500],
//                 
//                 new byte[500],
//                 new byte[500],
//                 
//                 new byte[500],
//                 new byte[500],
//                 new byte[51],
//                 new byte[51],
//                 new byte[51],
//                 
//                 new byte[51],
//                 new byte[516],
//                 new byte[516],
//                 
//                 new byte[516],
//                 new byte[166],
//                 new byte[166],
//                 new byte[196]
//             });
//             //Act
//             shittyDatagramPacker.Send(new IPEndPoint(1,1),messages );
//             
//             //Assert
//             int numbersOfPackages = mockUdpSender.GetNumbersOfContainers();
//             mockUdpSender.PrintStatistics();
//             Assert.AreEqual(8, numbersOfPackages);
//         }
//         
//         [Test]
//         public void MessagesContainerIndexLengthTest()
//         {
//             //Arrange
//             MessagesContainer messagesContainer = new MessagesContainer();
//             //Act
//             byte[] data = ZeroFormatterSerializer.Serialize(messagesContainer);
//             //Assert
//             Assert.AreEqual(MessagesContainer.IndexLength, data.Length);
//         }
//         
//         [Test]
//         public void MessagesContainerIndexLengthTest2()
//         {
//             //Arrange
//             MessagesContainer messagesContainer = new MessagesContainer()
//             {
//                 Messages = new []{new byte[200], new byte[200]}
//             };
//             //Act
//             byte[] data = ZeroFormatterSerializer.Serialize(messagesContainer);
//             //Assert
//             Assert.AreEqual(MessagesContainer.IndexLength+200+4+200+4, data.Length);
//         }
//         
//         [Test]
//         public void MessagesContainerIndexLengthTest3()
//         {
//             //Arrange
//             MessagesContainer messagesContainer = new MessagesContainer()
//             {
//                 Messages = new []{new byte[200], new byte[200], new byte[200], new byte[200]}
//             };
//             //Act
//             byte[] data = ZeroFormatterSerializer.Serialize(messagesContainer);
//             //Assert
//             Assert.AreEqual(MessagesContainer.IndexLength+4+200+4+200+4+200+4+200, data.Length);
//         }
//     }
// }