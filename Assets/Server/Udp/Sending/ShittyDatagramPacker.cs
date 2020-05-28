using System;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

namespace Server.Udp.Sending
{
    public class ShittyDatagramPacker
    {
        private readonly int mtu;
        private readonly IUdpSender udpSender;

        public ShittyDatagramPacker(int mtu, IUdpSender udpSender)
        {
            if (mtu < 500)
            {
                throw new Exception($"Размер mtu слишком мал {mtu}");
            }
            
            this.mtu = mtu;
            this.udpSender = udpSender;
        }

        public void Send([NotNull] IPEndPoint ipEndPoint, [NotNull] List<byte[]> messages)
        {
            int nextMessageIndex = 0;
            int currentLengthInBytes = 0;
            int currentDatagramStartIndex = 0;

            foreach (var message in messages)
            {
                if (message.Length > mtu)
                {
                    throw new Exception($"Длина сообщения больше, чем mtu {message.Length}");
                }
            }
            
            //Пройти по всем сообщениям для этого игрока
            while (nextMessageIndex != messages.Count)
            {
                Console.WriteLine($"1 {nameof(nextMessageIndex)} {nextMessageIndex}");
                //Найти кол-во сообщений, которые можно уместить в контейнер
                
                while (true)
                {
                    //Сообщения кончились?
                    if (nextMessageIndex == messages.Count)
                    {
                        break;
                    }

                    //Можно добавить ещё одно? 
                    if (currentLengthInBytes + messages[nextMessageIndex].Length > mtu)
                    {
                        break;
                    }
                    
                    currentLengthInBytes += messages[nextMessageIndex].Length;
                    nextMessageIndex++;
                }
                Console.WriteLine($"2 {nameof(nextMessageIndex)} {nextMessageIndex}");
                
                int messagesCount = nextMessageIndex - currentDatagramStartIndex;
                
                Console.WriteLine($"1 {nameof(messagesCount)} {messagesCount}");
                
                //Положить сообщения в контейнер
                MessagesContainer messagesContainer = new MessagesContainer();
                messagesContainer.Messages = new byte[messagesCount][];
                messages.CopyTo(currentDatagramStartIndex, messagesContainer.Messages, 0, messagesCount);
                
                //Сериализовать контейнер
                byte[] datagram = ZeroFormatterSerializer.Serialize(messagesContainer);
                
                //Отправить контейнер
                udpSender.Send(datagram, ipEndPoint);
                
                //Обновить значения временных переменных
                currentDatagramStartIndex = nextMessageIndex;
                currentLengthInBytes = 0;
            }
        }
    }
}