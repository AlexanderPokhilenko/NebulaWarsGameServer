using System;
using System.Collections.Generic;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

namespace Server.Udp.Sending
{
    public class MockUdpSender: IUdpSender
    {
        private readonly List<MessagesPack> containers = new List<MessagesPack>();
        
        public void Send(byte[] serializedContainer, IPEndPoint endPoint)
        {
            Console.WriteLine("Отправка контейнера размером "+serializedContainer.Length+"\n\n\n\n");
            MessagesPack container = ZeroFormatterSerializer
                .Deserialize<MessagesPack>(serializedContainer);
            if (container.Messages == null)
            {
                throw new Exception("Пустой контейнер");
            }
            containers.Add(container);
        }

        public List<MessagesPack> GetAllMessages()
        {
            return containers;
        }
        
        public int GetNumbersOfContainers()
        {
            return containers.Count;
        }

        public void PrintStatistics()
        {
            foreach (var container in containers)
            {
                var data = ZeroFormatterSerializer.Serialize(container);
                Console.WriteLine($"\n\n\n\nКонтейнер размером "+data.Length);
                foreach (var message in container.Messages)
                {
                    Console.WriteLine("Сообщение размером "+message.Length);
                }
            }
        }
    }
}