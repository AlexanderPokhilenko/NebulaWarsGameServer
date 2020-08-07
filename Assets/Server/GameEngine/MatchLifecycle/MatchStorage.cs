using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Code.Common;
using JetBrains.Annotations;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// Хранит таблицу текущих матчей.
    /// </summary>
    public class MatchStorage
    {
        //matchId match
        private readonly ConcurrentDictionary<int, Match> matches;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchStorage));
      
        public MatchStorage()
        {
            matches = new ConcurrentDictionary<int, Match>();
        }

        public void AddMatch(Match match)
        {
            if (matches.TryAdd(match.matchId, match))
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
        [CanBeNull]
        public Match DequeueMatch(int matchId)
        {
            log.Info("Попытка удалить матч "+matchId);
            if (matches.TryRemove(matchId, out Match match))
            {
                log.Info($"Матч удалён {nameof(matchId)} {matchId}.");
                return match;
            }
            
            log.Error($"Попытка удалить матч, которого нет {nameof(matchId)} {matchId}");
            return null;
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