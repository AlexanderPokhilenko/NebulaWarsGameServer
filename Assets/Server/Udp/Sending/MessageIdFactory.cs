using System.Collections.Generic;

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
        private readonly Dictionary<ushort, uint> messageIds = new Dictionary<ushort, uint>();

        public void AddPlayer(ushort playerId)
        {
            
        }

        public void RemovePlayer(ushort playerId)
        {
            
        }
        
        public uint CreateMessageId(ushort playerId)
        {
            if (!messageIds.ContainsKey(playerId))
            {
                messageIds.Add(playerId, 0);
            }

            uint lastMessageId = messageIds[playerId];
            lastMessageId++;
            messageIds[playerId] = lastMessageId;
            return lastMessageId;
        }
    }
}