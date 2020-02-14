using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Udp;

//TODO перейти на MessageContainer

namespace OldServer.Experimental.Udp.ReliableUdp
{
    public static class ReliableUdpMessagesStorage
    {
        //<playerId, <messageId, Message>>
        private static readonly ConcurrentDictionary<int, Dictionary<uint, Message>> UnconfirmedMessages
            =new ConcurrentDictionary<int, Dictionary<uint, Message>>();

        public static void AddMessage(int playerId, Message message)
        {
            if (!UnconfirmedMessages.ContainsKey(playerId))
            {
                if (UnconfirmedMessages.TryAdd(playerId, new Dictionary<uint, Message>()))
                {
                    //Создана структура данных для хранения сообщений для конкретного игрока
                }
                else
                {
                    throw new Exception("Не удалось добавить словарь в ReliableUdp для игрока с playerId="+playerId);
                }
            }
            
            UnconfirmedMessages[playerId].Add(message.MessageId, message);
        }

        public static Dictionary<uint, Message>.ValueCollection GetReliableMessages(int playerId)
        {
            return UnconfirmedMessages[playerId].Values;
        }

        public static void RemoveMessage(int playerId, uint messageId)
        {
            UnconfirmedMessages[playerId].Remove(messageId);
        }
    }
}