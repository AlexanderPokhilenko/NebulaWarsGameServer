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
      
        public MatchStorage()
        {
            matches = new ConcurrentDictionary<int, Match>();
        }

        public void RemoveMatch(int matchId)
        {
            if (matches.ContainsKey(matchId))
            {
                if (matches.TryRemove(matchId, out _))
                {
                    //намана   
                    log.Info($"Матч удалён {nameof(matchId)} {matchId}.");
                }
                else
                {
                    //не намана
                    throw new Exception($"Не удалось убрать матч из словаря {nameof(matchId)} {matchId}");
                }
            }
            else
            {
                throw new Exception($"Попытка удалить матч, которого нет {nameof(matchId)} {matchId}");
            }
        }

        public bool HasPlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        public bool HasMatch(int matchId)
        {
            return matches.ContainsKey(matchId);
        }

        public IEnumerable<Match> GetAllMatches()
        {
            return matches.Values;
        }
        
        public bool TryGetMatchByPlayerId(int playerId, out Match match)
        {
            throw new NotImplementedException();
        }

        public void PingTryAddIpEndPoint(int matchId, int playerId, IPEndPoint ipEndPoint)
        {
            if (matches.TryGetValue(matchId, out var match))
            {
                match.PingTryAddIpEndPoint(playerId, ipEndPoint);
            }
            else
            {
                
            }
        }

        public void RemoveRudpMessage(uint messageIdToConfirm)
        {
           throw new NotImplementedException();
        }


        public bool TryGetIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            if (matches.TryGetValue(matchId, out var match))
            {
                if (match.TryGetIpEndPoint(playerId, out ipEndPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                ipEndPoint = null;
                return false;
            }
        }
    }
}