using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Code.Common;
using JetBrains.Annotations;

namespace Server.GameEngine
{
    /// <summary>
    /// Хранит таблицу текущих матчей.
    /// </summary>
    public class MatchStorage
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchStorage));
        
        //matchId match
        private readonly ConcurrentDictionary<int, Match> matches;
      
        public MatchStorage()
        {
            matches = new ConcurrentDictionary<int, Match>();
        }

        public void AddMatch(Match match)
        {
            if (matches.TryAdd(match.MatchId, match))
            {
                //ignore
            }
            else
            {
                throw new Exception("Не удалось добавить матч в коллекцию.");
            }
        }
        
        /// <summary>
        /// Удаляет матч из коллекции.
        /// </summary>
        public Match DequeueMatch(int matchId)
        {
            if (matches.TryRemove(matchId, out Match match))
            {
                log.Info($"Матч удалён {nameof(matchId)} {matchId}.");
                return match;
            }
            else
            {
                throw new Exception($"Попытка удалить матч, которого нет {nameof(matchId)} {matchId}");
            }
        }

        /// <summary>
        /// Перед созданием матча.
        /// </summary>
        public bool HasPlayer(int playerId)
        {
            foreach (Match match in matches.Values)
            {
                if (match.HasPlayer(playerId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Перед созданием матча.
        /// </summary>
        public bool HasMatch(int matchId)
        {
            return matches.ContainsKey(matchId);
        }

        public IEnumerable<Match> GetAllMatches()
        {
            return matches.Values;
        }

        public bool TryGetMatch(int matchId, out Match match)
        {
            return matches.TryGetValue(matchId, out match);
        }
    }
}