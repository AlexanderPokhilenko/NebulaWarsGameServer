using System;
using System.Collections.Generic;
using log4net;

namespace Server.GameEngine
{
    /// <summary>
    /// Создаёт и уничтожает матчи, которые были добавлены в соответствующие очереди. 
    /// </summary>
    public class MatchLifeCycleManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchLifeCycleManager));

        private readonly MatchStorage matchStorage;
        private readonly MatchCreator matchCreator;
        private readonly MatchRemover matchRemover;
        
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