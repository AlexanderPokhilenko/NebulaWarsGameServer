using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
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
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(ByteArrayRudpStorage));
        
        private readonly ConcurrentDictionary<uint, int> messageIdPlayerId;
        private readonly ConcurrentDictionary<int, Dictionary<uint, byte[]>> unconfirmedMessages;

        private ByteArrayRudpStorage()
        {
            messageIdPlayerId = new ConcurrentDictionary<uint, int>();
            unconfirmedMessages = new ConcurrentDictionary<int, Dictionary<uint, byte[]>>();
        }
        
        public void AddMessage(int playerId, MessageWrapper messageWrapper)
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

            byte[] serializedMessage = ZeroFormatterSerializer.Serialize(messageWrapper);
            unconfirmedMessages[playerId].Add(messageWrapper.MessageId, serializedMessage);
            messageIdPlayerId.TryAdd(messageWrapper.MessageId, playerId);
        }
        
        public void AddMessage(int playerId, uint messageId, byte[] serializedMessage) 
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
            
            unconfirmedMessages[playerId].Add(messageId, serializedMessage);
            messageIdPlayerId.TryAdd(messageId, playerId);
        }

        public Dictionary<uint, byte[]>.ValueCollection GetReliableMessages(int playerId)
        {
            //TODO тут почему-то бросается исключение
            try
            {
                if (unconfirmedMessages.ContainsKey(playerId))
                {
                    return unconfirmedMessages[playerId].Values;
                }
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(GetReliableMessages)} брошено исключение "+e.Message);
            }
            
            return null;
        }

        public void RemoveMessage(uint confirmedMessageNumber)
        {
            if (messageIdPlayerId.ContainsKey(confirmedMessageNumber))
            {
                if(!messageIdPlayerId.TryRemove(confirmedMessageNumber, out int playerId))
                    throw new Exception("Ошибка удаления сообщения");
                
                if(!unconfirmedMessages.TryGetValue(playerId, out var dict) || !dict.Remove(confirmedMessageNumber))
                    throw new Exception("Ошибка удаления сообщения");
                
                Log.Info("Успешное удаление сообщения из коллекции");
            }
            else
            {
                throw new Exception($"Не удалось удалить сообщение из коллекции " +
                                    $"{nameof(confirmedMessageNumber)}={confirmedMessageNumber}");
            }
        }
    }
}