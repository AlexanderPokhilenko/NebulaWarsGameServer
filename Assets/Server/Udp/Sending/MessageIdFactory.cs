using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Udp.Sending
{
    /// <summary>
    /// Номера сообщений для каждого игрока идут подряд.
    /// Благодря этому клиент будет понимать как часто теряются сообщения.
    /// </summary>
    public class MessageIdFactory
    {
        /// <summary>
        /// playerId + lastMessageId
        /// </summary>
        private readonly Dictionary<Tuple<int, ushort>, uint> messageIds = new Dictionary<Tuple<int, ushort>, uint>();

        public void AddPlayer(int matchId, ushort playerId)
        {
            messageIds.Add(new Tuple<int, ushort>(matchId, playerId), 0);
        }

        public void RemovePlayer(int matchId, ushort playerId)
        {
            messageIds.Remove(new Tuple<int, ushort>(matchId, playerId));
        }
        
        public uint CreateMessageId(int matchId, ushort playerId)
        {
            var key = new Tuple<int, ushort>(matchId, playerId);
            uint lastMessageId = messageIds[key];
            lastMessageId++;
            messageIds[key] = lastMessageId;
            return lastMessageId;
        }
    }
}