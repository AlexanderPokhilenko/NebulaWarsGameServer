using System.Collections.Generic;
using Code.Common;
using Code.Common.Logger;
using SharedSimulationCode;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// Создаёт и уничтожает матчи, которые были добавлены в соответствующие очереди. 
    /// </summary>
    public class MatchLifeCycleManager
    {
        private readonly MatchesStorage matchesStorage;
        private readonly MatchCreator matchCreator;
        private readonly MatchRemover matchRemover;
        private static readonly ILog Log = LogManager.CreateLogger(typeof(MatchLifeCycleManager));
        
        public MatchLifeCycleManager(MatchesStorage matchesStorage, MatchCreator matchCreator, MatchRemover matchRemover)
        {
            this.matchesStorage = matchesStorage;
            this.matchCreator = matchCreator;
            this.matchRemover = matchRemover;
        }

        public void UpdateMatchesLifeStatus()
        {
            List<ServerMatchSimulation> createdMatches = matchCreator.CreateMatches();
            foreach (var match in createdMatches)
            {
                matchesStorage.AddMatch(match);
            }
            matchRemover.DeleteFinishedMatches();
        }
    }
}