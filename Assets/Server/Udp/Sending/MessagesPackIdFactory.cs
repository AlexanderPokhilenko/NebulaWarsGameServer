using System;
using System.Collections.Generic;
using Code.Common;

namespace Server.Udp.Sending
{
    public class MessagesPackIdFactory
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(MessagesPackIdFactory));
        private readonly Dictionary<Tuple<int, ushort>, int> lastDatagramIds = new Dictionary<Tuple<int, ushort>, int>();
        public void AddPlayer(int matchId, ushort playerId)
        {
            lastDatagramIds.Add(new Tuple<int, ushort>(matchId, playerId),0);
        }
        
        public void RemovePlayer(int matchId, ushort playerId)
        {
            lastDatagramIds.Remove(new Tuple<int, ushort>(matchId, playerId));
        }
        
        public int CreateId(int matchId, ushort playerId)
        {
            var key = new Tuple<int, ushort>(matchId, playerId);
            int id = lastDatagramIds[key];
            lastDatagramIds[key] = id + 1;
            return id;
        }
    }
}