using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using log4net;

//TODO убрать lock и выяснить в чём проблема
//конкурентной коллекции должно быть достаточно 

namespace Server.Udp.Storage
{
    /// <summary>
    /// Хранит сообщения, доставка которых не была подтверждена.
    /// </summary>
    public class ByteArrayRudpStorage
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ByteArrayRudpStorage));
        
        private readonly object lockObj = new object();
        
        //messageId PlayerId
        private readonly ConcurrentDictionary<uint, int> messageIdPlayerId;
        //PlayerId messageId message
        private readonly ConcurrentDictionary<int, Dictionary<uint, byte[]>> unconfirmedMessages;

        public ByteArrayRudpStorage()
        {
            messageIdPlayerId = new ConcurrentDictionary<uint, int>();
            unconfirmedMessages = new ConcurrentDictionary<int, Dictionary<uint, byte[]>>();
        }
        
        public void AddReliableMessage(int matchId, int playerId, uint messageId, byte[] serializedMessage) 
        {
            lock (lockObj)
            {
                if (!unconfirmedMessages.ContainsKey(playerId))
                {
                    if (unconfirmedMessages.TryAdd(playerId, new Dictionary<uint, byte[]>()))
                    {
                        //структура данных для хранения сообщений для конкретного игрока создана
                    }
                    else
                    {
                        throw new Exception("Не удалось добавить словарь в ReliableUdp для игрока с PlayerId=" + playerId);
                    }
                }
                unconfirmedMessages[playerId].Add(messageId, serializedMessage);
            }
            messageIdPlayerId.TryAdd(messageId, playerId);
        }

        public bool TryRemoveMessage(uint confirmedMessageNumber)
        {
            if (messageIdPlayerId.ContainsKey(confirmedMessageNumber))
            {
                if (messageIdPlayerId.TryRemove(confirmedMessageNumber, out int playerId))
                {
                    lock (lockObj)
                    {
                        if (unconfirmedMessages.TryGetValue(playerId, out var dict))
                        {
                            if (dict.Remove(confirmedMessageNumber))
                            {
                                // Log.Info("Успешное удаление сообщения из коллекции");
                                return true;
                            }
                            else
                            {
                                throw new Exception("Ошибка удаления сообщения");
                            }
                        } else
                        {
                            throw new Exception("Ошибка удаления сообщения");
                        }
                    }    
                }
                else
                {
                    throw new Exception("Ошибка удаления сообщения");
                }
            }
            else
            {
                return false;
            }
        }

        [CanBeNull]
        public Dictionary<uint, byte[]>.ValueCollection GetAllMessagesForPlayer(int playerId)
        {
            lock (lockObj)
            {
                if (unconfirmedMessages.ContainsKey(playerId))
                {
                    return unconfirmedMessages[playerId].Values;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}