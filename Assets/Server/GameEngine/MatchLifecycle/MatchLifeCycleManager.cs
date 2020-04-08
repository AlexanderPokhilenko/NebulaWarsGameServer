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

        private readonly MatchCreator matchCreator;
        private readonly MatchRemover matchRemover;
        
        public MatchLifeCycleManager(MatchStorage matchStorage)
        {
            matchRemover = new MatchRemover(matchStorage);
            MatchFactory matchFactory = new MatchFactory(matchRemover);
            matchCreator = new MatchCreator(matchFactory);
        }

        public void UpdateMatchesLifeStatus()
        {
            //TODO сохранить матчи
            List<Match> matches = matchCreator.CreateMatches();
            matchRemover.DeleteFinishedBattles();
            throw new NotImplementedException();
        }
    }
}