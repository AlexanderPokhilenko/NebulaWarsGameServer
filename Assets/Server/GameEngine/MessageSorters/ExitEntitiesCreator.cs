﻿using System.Collections.Concurrent;
using System.Linq;
using log4net;

namespace Server.GameEngine.Experimental
{
    /// <summary>
    /// Создаёт сущности о преждевременном покидании боя в контекстах.
    /// </summary>
    public class ExitEntitiesCreator
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ExitEntitiesCreator));
        
        private readonly ConcurrentBag<int> concurrentBag = new ConcurrentBag<int>();
        private readonly MatchStorage matchStorage;
        
        public ExitEntitiesCreator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public void AddExitMessage(int playerId)
        {
            log.Warn(nameof(AddExitMessage));
            if (!concurrentBag.Contains(playerId))
            {
                log.Warn(nameof(AddExitMessage)+" добавление в список");
                concurrentBag.Add(playerId);
            }
        }

        public void Create()
        {
            while (!concurrentBag.IsEmpty)
            {
                if (concurrentBag.TryTake(out int playerId))
                {
                    if (matchStorage.TryGetMatchByPlayerId(playerId, out var match))
                    {
                        match.AddPlayerExit(playerId);
                    }
                    else
                    {
                        log.Error($"Не удалось найти матч для игрока {nameof(playerId)} {playerId}");
                    }
                }
            }   
        }
    }
}