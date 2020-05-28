using System;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using NUnit.Framework;
using UnityEngine;
using ZeroFormatter;

namespace Tests
{
    public class ZeroFormatterTests
    {
        public ZeroFormatterTests()
        { 
            // ZeroFormatterInitializer.Register();
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
        
        [Test]
        public void Test3()
        {
            //Arrange
            PositionsMessage testMessageClass1 = new PositionsMessage
            {
                PlayerEntityId = 54,
                RadiusInfo = new Dictionary<ushort, ushort>()
                {
                    {1,1}
                },
                EntitiesInfo = new Dictionary<ushort, ViewTransform>()
                {
                    {5, new ViewTransform()}
                }
            };
            
            //Act
            byte[] serialized = ZeroFormatterSerializer.Serialize(testMessageClass1);
            
            //Assert
            Assert.AreEqual(32, serialized.Length);
        }  
        
        [Test]
        public void Test4()
        {
            //Arrange
            ViewTransform viewTransform = new ViewTransform();
            //Act
            byte[] serialized = ZeroFormatterSerializer.Serialize(viewTransform);
            
            //Assert
            Assert.AreEqual(13, serialized.Length);
        }
        
        [Test]
        public void Test5()
        {
            //Arrange
            ViewTransform viewTransform = new ViewTransform();
            //Act
            byte[] serialized = ZeroFormatterSerializer.Serialize(viewTransform);
            
            //Assert
            Assert.AreEqual(13, serialized.Length);
        }
        
        [Test]
        public void Test6()
        {
            //Arrange
            float pi = Mathf.PI;
            
            //Act
            ushort half = Mathf.FloatToHalf(pi);
            float restoredPi = Mathf.HalfToFloat(half);

            Console.WriteLine(pi);
            Console.WriteLine(restoredPi);
            
            //Assert
            Assert.IsTrue(Mathf.Abs(pi-restoredPi)<0.2);
        }
        
        [Test]
        public void Test7()
        {
            //Arrange
            ViewTransform viewTransform = new ViewTransform(45.25f, -4.3f, 180f, ViewTypeId.Asteroid100);
            
            //Act
            var data = ZeroFormatterSerializer.Serialize(viewTransform);
            var viewTransform2 = ZeroFormatterSerializer.Deserialize<ViewTransform>(data);
            
            Console.WriteLine(viewTransform2.X);
            Console.WriteLine(viewTransform2.Y);
            Console.WriteLine(viewTransform2.Angle);
            //Assert
            
        }
    }
}