using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Server.Udp.Storage;
using UnityEngine;
using Random = System.Random;

namespace Tests.Rudp
{
    public class RudpTests
    {
        /// <summary>
        /// В хранилище сообщения нормально добавляются и извлекаются.
        /// </summary>
        [Test]
        public void Test1()
        {
            //Arrange
            ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();
            int playerId = 5;
            uint messageId = 684;
            byte[] message = new byte[5];
            
            //Act
            byteArrayRudpStorage.AddMessage(playerId, messageId, message);
            
            //Assert
            var messagesForPlayer = byteArrayRudpStorage.GetAllMessagesForPlayer(playerId);
            Assert.IsNotNull(messagesForPlayer);
            Assert.AreEqual(1, messagesForPlayer.Count);
            Assert.AreSame(message, messagesForPlayer.Single());
        }
        
        
        /// <summary>
        /// В хранилище сообщения нормально добавляются и извлекаются в больших количествах.
        /// </summary>
        [Test]
        public void Test2()
        {
            //Arrange
            ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();
            Random random = new Random();
            int countOfMessages = 100;
            
            //Act
            for (int i = 0; i < countOfMessages; i++)
            {
                int playerId = random.Next();
                uint messageId = (uint) i;
                byte[] message = new byte[5];
                byteArrayRudpStorage.AddMessage(playerId, messageId, message);
            }
            
            //Assert
            int countOfMessages1 = byteArrayRudpStorage.GetCountOfMessages1();
            int countOfMessages2 = byteArrayRudpStorage.GetCountOfMessages2();
            Assert.AreEqual(countOfMessages, countOfMessages1);
            Assert.AreEqual(countOfMessages1, countOfMessages2);
        }
        
        /// <summary>
        /// В хранилище сообщения нормально добавляются и извлекаются в больших количествах из разных потоков.
        /// </summary>
        [Test]
        public void Test3()
        {
            //Arrange
            ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();
            int countOfMessages = 50;
            int playerId = 64;
            ConcurrentQueue<string> logs = new ConcurrentQueue<string>();
            //Act

            Task.Run(() =>
            {
                logs.Enqueue($"запуск ");
                while (true)
                {
                    var messages = byteArrayRudpStorage.GetAllMessagesForPlayer(playerId);
                    if(messages!=null)
                    {
                        logs.Enqueue($"{nameof(messages.Count)} {messages.Count}");
                        foreach (var message in messages)
                        {
                            int length = message.Length;
                        }

                        if (messages.Count == countOfMessages)
                        {
                            break;
                        }
                    }
                }
              
            }).ConfigureAwait(false);
            
            for (int i = 0; i < countOfMessages; i++)
            {
                uint messageId = (uint) i;
                byte[] message = new byte[5];
                byteArrayRudpStorage.AddMessage(playerId, messageId, message);
                logs.Enqueue("Добавлено сообщение "+i);
            }
            
            //Assert
            while (!logs.IsEmpty)
            {
                if (logs.TryDequeue(out string log))
                {
                    // Debug.Log(log);
                    Console.WriteLine(log);
                }
            }
        }

        [Test]
        public void Test4()
        {
            
        }
    }
}