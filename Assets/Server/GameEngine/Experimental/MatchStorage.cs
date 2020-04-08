using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    /// <summary>
    /// Хранит таблицу текущих матчей и игроков
    /// </summary>
    public class MatchStorage
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MatchStorage));
        
        //MatchId Match
        private readonly ConcurrentDictionary<int, Match> matches;
        //accountId MatchId
        private readonly ConcurrentDictionary<int, int> activePlayers;

        public MatchStorage()
        {
            matches = new ConcurrentDictionary<int, Match>();
            activePlayers = new ConcurrentDictionary<int, int>();
        }

        public void RemoveMatch(int matchId)
        {
            throw new NotImplementedException();
        }

        public bool HasPlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        public bool HasMatch(int matchDataMatchId)
        {
            throw new NotImplementedException();
        }
    }
}