using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Code.Common;
using JetBrains.Annotations;

//TODO убрать lock и выяснить в чём проблема
//конкурентной коллекции должно быть достаточно 

namespace Server.GameEngine.Rudp
{
    /// <summary>
    /// Хранит сообщения, доставка которых не была подтверждена.
    /// Способ хранения можно улучшить использую группировку.
    /// </summary>
    public class ByteArrayRudpStorage
    {
        private readonly object lockObj = new object();
        private readonly ILog log = LogManager.CreateLogger(typeof(ByteArrayRudpStorage));
        //matchId playerId messageId message
        private readonly ConcurrentDictionary<int, Dictionary<int, Dictionary<uint, byte[]>>> unconfirmedMessages;
        
        public ByteArrayRudpStorage()
        {
            unconfirmedMessages = new ConcurrentDictionary<int, Dictionary<int, Dictionary<uint, byte[]>>>();
        }
        
        public void AddReliableMessage(int matchId, int playerId, uint messageId, byte[] serializedMessage) 
        {
            lock (lockObj)
            {
                TryCreateMatchDictionary(matchId);
                TryCreatePlayerDictionary(matchId, playerId);
                unconfirmedMessages[matchId][playerId].Add(messageId, serializedMessage);
            }
        }

        public void RemoveMatchMessages(int matchId)
        {
            log.Info($"Удаление сообщений для матча {nameof(matchId)} {matchId}.");
            lock (lockObj)
            {
                if(unconfirmedMessages.TryRemove(matchId, out var playersDict))
                {
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

        public bool TryRemoveMessage(int matchId, ushort playerId, uint messageIdToConfirm)
        {
            lock (lockObj)
            {
                if (unconfirmedMessages[matchId][playerId].ContainsKey(messageIdToConfirm))
                {
                    unconfirmedMessages[matchId][playerId].Remove(messageIdToConfirm);
                    return true;
                }

                return false;
            }
        }
    }
}