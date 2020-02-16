using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NetworkLibrary.NetworkLibrary.Udp;

//TODO перейти на MessageContainer

namespace OldServer.Experimental.Udp.Storage
{
    public static class ReliableUdpStorage
    {
        // ReSharper disable once InconsistentNaming
        private static readonly ConcurrentDictionary<uint, int> MessageId_PlayerId = new ConcurrentDictionary<uint, int>();
        //<playerId, <messageId, Message>>
        private static readonly ConcurrentDictionary<int, Dictionary<uint, Message>> UnconfirmedMessages
            =new ConcurrentDictionary<int, Dictionary<uint, Message>>();

        public static void AddMessage(int playerId, Message message)
        {
            if (!UnconfirmedMessages.ContainsKey(playerId))
            {
                if (UnconfirmedMessages.TryAdd(playerId, new Dictionary<uint, Message>()))
                {
                    //структура данных для хранения сообщений для конкретного игрока создана
                }
                else
                {
                    throw new Exception("Не удалось добавить словарь в ReliableUdp для игрока с playerId="+playerId);
                }
            }
            
            UnconfirmedMessages[playerId].Add(message.MessageId, message);
            MessageId_PlayerId.TryAdd(message.MessageId, playerId);
        }

        public static Dictionary<uint, Message>.ValueCollection GetReliableMessages(int playerId)
        {
            return UnconfirmedMessages[playerId].Values;
        }

        public static void RemoveMessage(uint confirmedMessageNumber)
        {
            if (MessageId_PlayerId.ContainsKey(confirmedMessageNumber))
            {
                MessageId_PlayerId.TryRemove(confirmedMessageNumber, out int playerId);
                UnconfirmedMessages.TryRemove(playerId, out var dict);    
            }
            else
            {
                throw new Exception($"Не удалось удалить сообщение из коллекции " +
                                    $"{nameof(confirmedMessageNumber)}={confirmedMessageNumber}");
            }
        }
    }
}