using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugins.submodules.SharedCode.Logger;
using UnityEngine;

namespace Server.Udp.Sending
{
    /// <summary>
    /// Номера сообщений для каждого игрока идут подряд.
    /// Благодря этому клиент будет понимать как часто теряются сообщения.
    /// </summary>
    public class MessageIdFactory
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(MessageIdFactory));
        /// <summary>
        /// matchId playerId + lastMessageId
        /// </summary>
        private readonly Dictionary<Tuple<int, ushort>, uint> messageIds = new Dictionary<Tuple<int, ushort>, uint>();

        public void AddPlayer(int matchId, ushort playerId)
        {
            log.Debug($"Создание счётчика id matchId={matchId} playerId={playerId}");
            messageIds.Add(new Tuple<int, ushort>(matchId, playerId), 0);
        }

        public void RemovePlayer(int matchId, ushort playerId)
        {
            messageIds.Remove(new Tuple<int, ushort>(matchId, playerId));
        }
        
        public uint CreateMessageId(int matchId, ushort playerId)
        {
            Tuple<int, ushort> key = new Tuple<int, ushort>(matchId, playerId);
            if(messageIds.TryGetValue(key, out uint lastMessageId))
            {
                lastMessageId++;
                messageIds[key] = lastMessageId;
                return lastMessageId;    
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (ushort tmpPlayerId in messageIds.Keys.Select(item => item.Item2))
                {
                    sb.Append(tmpPlayerId + " ");
                }
                
                string message = $"Не ожидается создание id сообщения для этого игрока. matchId = {matchId}, " +
                                 $"playerId = {playerId} ." +
                                 $" Есть игроки с tmpId "+sb;
                
                throw new Exception(message);
            }
        }
    }
}