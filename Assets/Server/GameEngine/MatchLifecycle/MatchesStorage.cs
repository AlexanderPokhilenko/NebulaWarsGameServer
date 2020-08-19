using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;
using Plugins.submodules.SharedCode.Logger;
using SharedSimulationCode.Systems;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// Хранит таблицу текущих матчей.
    /// </summary>
    public class MatchesStorage
    {
        //matchId match
        private readonly ConcurrentDictionary<int, ServerMatchSimulation> matches;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchesStorage));
      
        public MatchesStorage()
        {
            matches = new ConcurrentDictionary<int, ServerMatchSimulation>();
        }

        public void AddMatch(ServerMatchSimulation serverMatch)
        {
            if (matches.TryAdd(serverMatch.matchId, serverMatch))
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
        public ServerMatchSimulation DequeueMatch(int matchId)
        {
            log.Info("Попытка удалить матч "+matchId);
            if (matches.TryRemove(matchId, out ServerMatchSimulation match))
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

        public IEnumerable<ServerMatchSimulation> GetAllMatches()
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