using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
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
            int index = 0;
            while(index <= messages.Count-1)
            {
                byte[] message = messages[index];
                if (message.Length + MessagesContainer.IndexLength + 4 > mtu)
                {
                    //Console.WriteLine("Длина сообщения слишком большая "+message.Length);
                    // throw new Exception($"Длина сообщения больше, чем mtu {message.Length}");
                    
                    // TODO убрать после добавления возможности разделять большие сообщения
                    {
                        MessagesContainer messagesContainer = new MessagesContainer
                        {
                            Messages = new[] {message}
                        };
                        byte[] data = ZeroFormatterSerializer.Serialize(messagesContainer);
                        udpSender.Send(data, ipEndPoint);
                        messages.RemoveAt(index);
                    }
                }
                else
                {
                    // //Console.WriteLine("Сообщение допустимой длины "+message.Length);
                    index++;
                }
            }

            //Console.WriteLine();
           

            //Суммарная длина нескольких сообщений в контейнере
            int currentPayloadLengthInBytes = 0;
            //Номер первого сообщения в контейнере
            int startMessageIndex = 0;
            for (int currentMessageIndex = 0; currentMessageIndex < messages.Count; currentMessageIndex++)
            {
                int messagesCount = currentMessageIndex - startMessageIndex;
                int currentMessageLength = messages[currentMessageIndex].Length;
                //Console.WriteLine($"{nameof(currentMessageLength)} {currentMessageLength} {nameof(messagesCount)} {messagesCount}");
                //Вместе с этим сообщением сериализованный контейкер поместитсяя в mtu?
                if (currentPayloadLengthInBytes + currentMessageLength + MessagesContainer.IndexLength + 4*(messagesCount+1)<= mtu)
                {
                    //Обновить длину полезной нагрузки
                    currentPayloadLengthInBytes += currentMessageLength;
                    //Console.WriteLine($"Обновление длины полезной нагрузки {nameof(currentPayloadLengthInBytes)} {currentPayloadLengthInBytes}");
                }
                else
                {
                    //Отправка полезной нагрузки
                    SendDatagram(ipEndPoint, messages, startMessageIndex, messagesCount);
                    //Обновить номер первого сообщения в контейнере
                    startMessageIndex = currentMessageIndex;
                    currentPayloadLengthInBytes = currentMessageLength;
                }
            }

            //Если последняя полезная нагрузка не была отправлена
            if (currentPayloadLengthInBytes != 0)
            {
                //Отправить не до конца заполненный контейнер
                SendDatagram(ipEndPoint, messages, startMessageIndex, messages.Count-startMessageIndex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SendDatagram(IPEndPoint ipEndPoint, List<byte[]> messages, int startMessageIndex, int messagesCount)
        {
            //Console.WriteLine($"Отправка полезной нагрузки {nameof(startMessageIndex)} {startMessageIndex} {nameof(messagesCount)} {messagesCount}");
            if (messagesCount == 0)
            {
                throw new Exception("Нельзя отправлять пустые контейнеры");
            }
            
            //Положить сообщения в контейнер
            MessagesContainer messagesContainer = new MessagesContainer();
            messagesContainer.Messages = new byte[messagesCount][];
            messages.CopyTo(startMessageIndex, messagesContainer.Messages, 0, messagesCount);
            //Сериализовать контейнер
            byte[] datagram = ZeroFormatterSerializer.Serialize(messagesContainer);
            //Отправить контейнер
            udpSender.Send(datagram, ipEndPoint);
        }
    }
}