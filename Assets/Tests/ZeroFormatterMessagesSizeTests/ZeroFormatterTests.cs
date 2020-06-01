using System;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using NUnit.Framework;
using UnityEngine;
using ZeroFormatter;
using Vector2 = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

namespace Tests
{
    /// <summary>
    /// Проверка размера сообщений после сериализации
    /// </summary>
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
        
        [Test]
        public void Test3()
        {
            /*
             *
             * [byteSize:int(4)][length:int(4)][elementOffset...:int(4 * length)][elements:T...]
             */
            //Arrange
            PositionsMessage testMessageClass1 = new PositionsMessage
            {
                //28 байт на индекс
                //2 байта
                PlayerEntityId = 54,
                RadiusInfo = new Dictionary<ushort, ushort>
                {
                    //2+2 байта на элемент
                    {1,1},
                    {2,1}
                }
                // EntitiesInfo = new Dictionary<ushort, ViewTransform>
                // {
                //     //2+7 байт на элемент
                //     {5, new ViewTransform()}
                // }
            };
            
            //Act
            byte[] serialized = ZeroFormatterSerializer.Serialize(testMessageClass1);
            
            //Assert
            Assert.AreEqual(30+4+4, serialized.Length);
        }  
      
        [Test]
        public void Test4()
        {
            //Arrange
            ViewTransform viewTransform = new ViewTransform();
            //Act
            byte[] serialized = ZeroFormatterSerializer.Serialize(viewTransform);
            
            //Assert
            Assert.AreEqual(7, serialized.Length);
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
            
            Debug.LogWarning(viewTransform2.X);
            Debug.LogWarning(viewTransform2.Y);
            Debug.LogWarning(viewTransform2.Angle);
            //Assert
            
        }
        
        [Test]
        public void Test8()
        {
            //Arrange
            PositionsMessage positionsMessage = new PositionsMessage()
            {
                EntitiesInfo = new Dictionary<ushort, ViewTransform>()
                {
                    {5, new ViewTransform()},
                    {3, new ViewTransform()},
                    {6, new ViewTransform()},
                    {8, new ViewTransform()},
                    {9, new ViewTransform()},
                },
                RadiusInfo = new Dictionary<ushort, ushort>()
                {
                    {1, 45},
                    {2, 45},
                    {3, 45},
                    {4, 45},
                    {5, 45}
                },
                PlayerEntityId = 54
            };
            
            //Act
            var data = ZeroFormatterSerializer.Serialize(positionsMessage);
            
            Debug.LogWarning(data.Length);
            var viewTransform2 = ZeroFormatterSerializer.Deserialize<PositionsMessage>(data);
            
            
            //Assert
            
        }
        
        [Test]
        public void Test9()
        {
            //Arrange
            Vector2 vector2 = new Vector2(55f,66f);
            
            //Act
            var data = ZeroFormatterSerializer.Serialize(vector2);
            var restoredVector2 = ZeroFormatterSerializer.Deserialize<Vector2>(data);

            //Assert
            Assert.AreEqual(vector2.X, restoredVector2.X);
            Assert.AreEqual(vector2.Y, restoredVector2.Y);
        }
    }
}