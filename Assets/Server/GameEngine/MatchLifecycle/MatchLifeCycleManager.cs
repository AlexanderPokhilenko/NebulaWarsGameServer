﻿using System.Collections.Generic;
using Code.Common;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// Создаёт и уничтожает матчи, которые были добавлены в соответствующие очереди. 
    /// </summary>
    public class MatchLifeCycleManager
    {
        private readonly MatchStorage matchStorage;
        private readonly MatchCreator matchCreator;
        private readonly MatchRemover matchRemover;
        private static readonly ILog Log = LogManager.CreateLogger(typeof(MatchLifeCycleManager));
        
        public MatchLifeCycleManager(MatchStorage matchStorage, MatchCreator matchCreator, MatchRemover matchRemover)
        {
            this.matchStorage = matchStorage;
            this.matchCreator = matchCreator;
            this.matchRemover = matchRemover;
        }

        public void UpdateMatchesLifeStatus()
        {
            List<Match> createdMatches = matchCreator.CreateMatches();
            foreach (var match in createdMatches)
            {
                matchStorage.AddMatch(match);
            }
            matchRemover.DeleteFinishedMatches();
        }
    }
}