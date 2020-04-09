using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using log4net;

//TODO убрать lock и выяснить в чём проблема
//конкурентной коллекции должно быть достаточно 

//TODO это, конечно, дичь, но стабильность важнее

//TODO при удалении матча почистить rudp хранилище
//TODO протестировать это чудо

namespace Server.Udp.Storage
{
    /// <summary>
    /// Хранит сообщения, доставка которых не была подтверждена.
    /// Способ хранения можно улучшить использую группировку.
    /// </summary>
    public class ByteArrayRudpStorage
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ByteArrayRudpStorage));
        
        private readonly object lockObj = new object();
        
        //messageId PlayerId
        private readonly ConcurrentDictionary<uint, (int matchId, int playerId)> messageIdPlayerId;
        
        //matchId playerId messageId message
        private readonly ConcurrentDictionary<int, Dictionary<int, Dictionary<uint, byte[]>>> unconfirmedMessages;
        
        public ByteArrayRudpStorage()
        {
            messageIdPlayerId = new ConcurrentDictionary<uint, (int matchId, int playerId)>();
            unconfirmedMessages = new ConcurrentDictionary<int, Dictionary<int, Dictionary<uint, byte[]>>>();
        }
        
        public void AddReliableMessage(int matchId, int playerId, uint messageId, byte[] serializedMessage) 
        {
            lock (lockObj)
            {
                TryCreateMatchDictionary(matchId);
                TryCreatePlayerDictionary(matchId, playerId);
                unconfirmedMessages[matchId][playerId].Add(messageId, serializedMessage);
                messageIdPlayerId.TryAdd(messageId, (matchId, playerId));
            }
        }

        public void RemoveMatchMessages(int matchId)
        {
            lock (lockObj)
            {
                if(unconfirmedMessages.TryRemove(matchId, out var playersDict))
                {
                    foreach (var playerDict in playersDict)
                    {
                        foreach (var messageId in playerDict.Value.Keys)
                        {
                            messageIdPlayerId.TryRemove(messageId, out _);
                        }
                    }
                }
            }
        }
        
        public bool TryRemoveMessage(uint messageId)
        {
            lock (lockObj)
            {
                if(messageIdPlayerId.TryRemove(messageId, out var value))
                {
                   
                        unconfirmedMessages[value.matchId][value.playerId].Remove(messageId);
                        return true;
                }
                else
                {
                    //сообщение с messageId нет в структуре данных
                    return false;
                }
            }
        }

        [CanBeNull]
        public byte[][] GetMessages(int matchId, int playerId)
        {
            lock (lockObj)
            {
                if (unconfirmedMessages.TryGetValue(matchId, out var playerDict))
                {
                    if (playerDict.ContainsKey(playerId))
                    {
                        return playerDict[playerId].Select(pair=>pair.Value).ToArray();
                    }
                }
                return null;
            }
        }
        
        private void TryCreateMatchDictionary(int matchId)
        {
            lock (lockObj)
            {
                if (!unconfirmedMessages.ContainsKey(matchId))
                {
                    if (unconfirmedMessages.TryAdd(matchId, new Dictionary<int, Dictionary<uint, byte[]>>()))
                    {
                        //структура данных для матча создана   
                    }
                    else
                    {
                        throw new Exception($"Не удалось добавить ReliableUdp в словарь для {nameof(matchId)} {matchId}");
                    }
                }
            }
        }

        private void TryCreatePlayerDictionary(int matchId, int playerId)
        {
            lock (lockObj)
            {
                if (!unconfirmedMessages[matchId].ContainsKey(playerId))
                {
                    unconfirmedMessages[matchId].Add(playerId, new Dictionary<uint, byte[]>());
                    //структура данных для игрока в матче создана
                }
            }
        }
    }
}