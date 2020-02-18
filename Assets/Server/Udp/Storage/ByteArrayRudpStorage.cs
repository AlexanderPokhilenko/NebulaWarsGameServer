using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

//TODO я не смог в ioc поэтому впихнул синглтон. не делайте так.

namespace Server.Udp.Storage
{
    public class ByteArrayRudpStorage
    {
        private static readonly Lazy<ByteArrayRudpStorage> Lazy = new Lazy<ByteArrayRudpStorage> (
            () => new ByteArrayRudpStorage());
        public static ByteArrayRudpStorage Instance => Lazy.Value;
        
        private readonly ConcurrentDictionary<uint, int> messageIdPlayerId;
        private readonly ConcurrentDictionary<int, Dictionary<uint, byte[]>> unconfirmedMessages;

        private ByteArrayRudpStorage()
        {
            messageIdPlayerId = new ConcurrentDictionary<uint, int>();
            unconfirmedMessages = new ConcurrentDictionary<int, Dictionary<uint, byte[]>>();
        }
        
        public void AddMessage(int playerId, Message message)
        {
            if (!unconfirmedMessages.ContainsKey(playerId))
            {
                if (unconfirmedMessages.TryAdd(playerId, new Dictionary<uint, byte[]>()))
                {
                    //структура данных для хранения сообщений для конкретного игрока создана
                }
                else
                {
                    throw new Exception("Не удалось добавить словарь в ReliableUdp для игрока с playerId="+playerId);
                }
            }

            byte[] serializedMessage = ZeroFormatterSerializer.Serialize(message);
            unconfirmedMessages[playerId].Add(message.MessageId, serializedMessage);
            messageIdPlayerId.TryAdd(message.MessageId, playerId);
        }

        public Dictionary<uint, byte[]>.ValueCollection GetReliableMessages(int playerId)
        {
            if (unconfirmedMessages.ContainsKey(playerId))
            {
                return unconfirmedMessages[playerId].Values;
            }
            return null;
        }

        public void RemoveMessage(uint confirmedMessageNumber)
        {
            if (messageIdPlayerId.ContainsKey(confirmedMessageNumber))
            {
                if(!messageIdPlayerId.TryRemove(confirmedMessageNumber, out int playerId))
                    throw new Exception("Ошибка удаления сообщения");
                
                if(!unconfirmedMessages.TryRemove(playerId, out var dict))
                    throw new Exception("Ошибка удаления сообщения");
            }
            else
            {
                throw new Exception($"Не удалось удалить сообщение из коллекции " +
                                    $"{nameof(confirmedMessageNumber)}={confirmedMessageNumber}");
            }
        }
    }
}