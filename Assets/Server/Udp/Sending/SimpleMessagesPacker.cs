using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Plugins.submodules.SharedCode.Logger;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;
using ZeroFormatter;

namespace Server.Udp.Sending
{
    public class ServerSendingJitterSimulation:IUdpSender
    {
        public void Send(byte[] serializedContainer, IPEndPoint endPoint)
        {
            
        }
    }

    public class SimpleMessagesPacker
    {
        private readonly int mtu;
        private readonly IUdpSender udpSender;
        private readonly MessagesPackIdFactory messagesPackIdFactory;
        private readonly ILog log = LogManager.CreateLogger(typeof(SimpleMessagesPacker));
        
        public SimpleMessagesPacker(int mtu, IUdpSender udpSender, MessagesPackIdFactory messagesPackIdFactory)
        {
            if (mtu < 500)
            {
                throw new Exception($"Размер mtu слишком мал {mtu}");
            }
            
            this.mtu = mtu;
            this.udpSender = udpSender;
            this.messagesPackIdFactory = messagesPackIdFactory;
        }

        public void Send(int matchId, ushort playerId,[NotNull] IPEndPoint ipEndPoint, [NotNull] List<byte[]> messages)
        {
            //Проверка на наличие слишком больших сообщений
            MessagesMtuExceedCheck(ipEndPoint,messages);
            //Суммарная длина нескольких сообщений в контейнере
            int currentPayloadLengthInBytes = 0;
            //Номер первого сообщения в контейнере
            int startMessageIndex = 0;
            for (int currentMessageIndex = 0; currentMessageIndex < messages.Count; currentMessageIndex++)
            {
                int messagesCount = currentMessageIndex - startMessageIndex;
                int currentMessageLength = messages[currentMessageIndex].Length;
                
                if (IsMessageFitsIntoPackage(currentPayloadLengthInBytes, currentMessageLength, messagesCount))
                {
                    //Обновить длину полезной нагрузки
                    currentPayloadLengthInBytes += currentMessageLength;
                }
                else
                {
                    //Отправка полезной нагрузки
                    SendDatagram(ipEndPoint, messages, startMessageIndex, messagesCount, messagesPackIdFactory.CreateId(matchId, playerId));
                    //Сбросить номер первого сообщения в контейнере
                    startMessageIndex = currentMessageIndex;
                    currentPayloadLengthInBytes = currentMessageLength;
                }
            }

            //Если последняя полезная нагрузка не была отправлена
            if (currentPayloadLengthInBytes != 0)
            {
                //Отправить не до конца заполненный контейнер
                SendDatagram(ipEndPoint, messages, startMessageIndex, messages.Count-startMessageIndex, messagesPackIdFactory.CreateId(matchId, playerId));
            }
        }

        private bool IsMessageFitsIntoPackage(int currentPayloadLengthInBytes, int currentMessageLength, 
            int payloadMessagesCount)
        {
            int dataLength = currentPayloadLengthInBytes + currentMessageLength;
            int indexLength = MessagesPack.IndexLength + 4 * (payloadMessagesCount + 1); 
            return dataLength + indexLength  <= mtu;
        }
        
        /// <summary>
        /// Если сообщение превышает размер mtu, то нужно бросать исключение или отправлять его отдельно.
        /// </summary>
        private void MessagesMtuExceedCheck(IPEndPoint ipEndPoint, List<byte[]> messages)
        {
            //Отправка сообщений, которые превышают  mtu отдельно
            int index = 0;
            while(index < messages.Count)
            {
                byte[] message = messages[index];
                if (message.Length + MessagesPack.IndexLength + 4 > mtu)
                {
                    //Console.WriteLine("Длина сообщения слишком большая "+message.Length);
                    MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(message);
                    log.Error($"MessageType = "+messageWrapper.MessageType);
                    log.Error($"NeedResponse = "+messageWrapper.NeedResponse);
                    throw new Exception($"Длина сообщения больше, чем mtu {message.Length}");
                    
                    //{
                    //    MessagesPack messagesPack = new MessagesPack
                    //    {
                    //        Messages = new[] {message}
                    //    };
                    //    byte[] data = ZeroFormatterSerializer.Serialize(messagesPack);
                    //    udpSender.Send(data, ipEndPoint);
                    //    messages.RemoveAt(index);
                    //}
                }
                else
                {
                    // //Console.WriteLine("Сообщение допустимой длины "+message.Length);
                    index++;
                }
            }
        } 
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SendDatagram(IPEndPoint ipEndPoint, List<byte[]> messages, int startMessageIndex, 
            int messagesCount, int packId)
        {
            if (messagesCount == 0)
            {
                throw new Exception("Нельзя отправлять пустые контейнеры");
            }
            
            //Положить сообщения в контейнер
            MessagesPack messagesPack = new MessagesPack();
            messagesPack.Messages = new byte[messagesCount][];
            messages.CopyTo(startMessageIndex, messagesPack.Messages, 0, messagesCount);
            //Установить номер контейнера для игрока
            messagesPack.Id = packId;
            //Сериализовать контейнер
            byte[] serializedDatagram = ZeroFormatterSerializer.Serialize(messagesPack);
            //Отправить контейнер
            udpSender.Send(serializedDatagram, ipEndPoint);
        }
    }
}