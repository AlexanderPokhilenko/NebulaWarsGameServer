using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Code.Common;
using JetBrains.Annotations;
using SharedSimulationCode;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// Хранит таблицу текущих матчей.
    /// </summary>
    public class MatchStorage
    {
        //matchId match
        private readonly ConcurrentDictionary<int, MatchSimulation> matches;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchStorage));
      
        public MatchStorage()
        {
            matches = new ConcurrentDictionary<int, MatchSimulation>();
        }

        public void AddMatch(MatchSimulation match)
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
        public MatchSimulation DequeueMatch(int matchId)
        {
            log.Info("Попытка удалить матч "+matchId);
            if (matches.TryRemove(matchId, out MatchSimulation match))
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

        public IEnumerable<MatchSimulation> GetAllMatches()
        {
            return matches.Values;
        }

        public bool TryGetMatchInputReceiver(int matchId, ref InputReceiver inputReceiver)
        {
            if (matches.TryGetValue(matchId, out var match))
            {
                inputReceiver = match.GetInputReceiver();
                return true;
            }

            return false;
        }
    }
}