using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using Server.Udp.Sending;
using ZeroFormatter;

namespace Tests
{
    public class DatagramPackingTests
    {
        public DatagramPackingTests()
        {
            //Нужно, чтобы сообщения сериализовались в массивы байт
            ZeroFormatterInitializer.Register();
        }
        
        [Test]
        public void NullMessagesList()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            
            //Act + Assert
            Assert.That(() =>
                {
                    shittyDatagramPacker.Send(new IPEndPoint(0, 0), null);
                }
                , Throws.Exception);
        }
        
        [Test]
        public void NegativeMtu()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = -1;
            
            //Act + Assert
            Assert.That(() =>
                {
                    ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
                }
                , Throws.Exception);
        }
        
        [Test]
        public void ZeroMtu()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 0;
            
            //Act + Assert
            Assert.That(() =>
                {
                    ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
                }
                , Throws.Exception);
        }
        
        [Test]
        public void EmptyMessagesList()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            List<byte[]> messages = new List<byte[]>();

            //Act
            shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
            
            //Assert
            int numbersOfPackages = mockUdpSender.GetNumbersOfPackages();
            Assert.AreEqual(0, numbersOfPackages);
        }
        
        
        [Test]
        public void OnePacket()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            List<byte[]> messages = new List<byte[]>
            {
                new byte[100]
            };

            //Act
            shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
            
            //Assert
            int numbersOfPackages = mockUdpSender.GetNumbersOfPackages();
            Assert.AreEqual(1, numbersOfPackages);
        }
        
        [Test]
        public void TenPacketsBy100Bytes()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            List<byte[]> messages = new List<byte[]>();
            for (int i = 0; i < 10; i++)
            {
                messages.Add(new byte[100]);
            }
            
            //Act
            shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
            
            //Assert
            int numbersOfPackages = mockUdpSender.GetNumbersOfPackages();
            Assert.AreEqual(1, numbersOfPackages);
        }
        
        [Test]
        public void TwelvePacketsBy100Bytes()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            List<byte[]> messages = new List<byte[]>();
            for (int i = 0; i < 12; i++)
            {
                messages.Add(new byte[100]);
            }
            
            //Act
            shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
            
            //Assert
            int numbersOfPackages = mockUdpSender.GetNumbersOfPackages();
            Assert.AreEqual(1, numbersOfPackages);
        }
        
        
        [Test]
        public void ThirteenPacketsBy100Bytes()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            List<byte[]> messages = new List<byte[]>();
            for (int i = 0; i < 13; i++)
            {
                messages.Add(new byte[100]);
            }
            
            //Act
            shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
            
            //Assert
            int numbersOfPackages = mockUdpSender.GetNumbersOfPackages();
            Assert.AreEqual(2, numbersOfPackages);
        }

        [Test]
        public void MessageSizeIs1MoreThanMtu()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            List<byte[]> messages = new List<byte[]>();
            for (int i = 0; i < 12; i++)
            {
                messages.Add(new byte[100]);
            }
            messages.Add(new byte[1]);
            
            //Act
            shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
            
            //Assert
            int numbersOfPackages = mockUdpSender.GetNumbersOfPackages();
            Assert.AreEqual(2, numbersOfPackages);
        }
        
        [Test]
        public void ThreeDatagrams()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            List<byte[]> messages = new List<byte[]>();
            for (int i = 0; i < 36; i++)
            {
                messages.Add(new byte[100]);
            }

            //Act
            shittyDatagramPacker.Send(new IPEndPoint(0,0), messages);
            
            //Assert
            int numbersOfPackages = mockUdpSender.GetNumbersOfPackages();
            Assert.AreEqual(3, numbersOfPackages);
        }
        
        [Test]
        public void TooLongMessage()
        {
            //Arrange
            MockUdpSender mockUdpSender = new MockUdpSender();
            int mtu = 1200;
            ShittyDatagramPacker shittyDatagramPacker = new ShittyDatagramPacker(mtu, mockUdpSender);
            List<byte[]> messages = new List<byte[]>(new[]
            {
                new byte[mtu+1]
            });

            //Act + Assert
            Assert.That(()=>
            {
                shittyDatagramPacker.Send(new IPEndPoint(0, 0), messages);
            }, Throws.Exception);
        }
    }
}