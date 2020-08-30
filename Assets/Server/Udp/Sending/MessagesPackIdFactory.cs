using System;
using System.Collections.Generic;
using Plugins.submodules.SharedCode.Logger;


namespace Server.Udp.Sending
{
    public class MessagesPackIdFactory
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(MessagesPackIdFactory));
        private readonly Dictionary<Tuple<int, ushort>, int> lastDatagramIds = new Dictionary<Tuple<int, ushort>, int>();
        
        public void AddPlayer(int matchId, ushort playerId)
        {
            lastDatagramIds.Add(new Tuple<int, ushort>(matchId, playerId), 1);
        }
        
        public void RemovePlayer(int matchId, ushort playerId)
        {
            lastDatagramIds.Remove(new Tuple<int, ushort>(matchId, playerId));
        }
        
        public int CreateId(int matchId, ushort playerId)
        {
            Tuple<int, ushort> key = new Tuple<int, ushort>(matchId, playerId);
            int id = lastDatagramIds[key];
            lastDatagramIds[key] = id + 1;
            if (id == 0)
            {
                throw new Exception("Нулевой номер");
            }
            
            return id;
        }
    }
}