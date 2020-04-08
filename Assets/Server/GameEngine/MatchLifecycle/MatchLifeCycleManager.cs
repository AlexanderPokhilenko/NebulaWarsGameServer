using log4net;

namespace Server.GameEngine
{
    //TODO название не очень
    /// <summary>
    /// Создаёт и уничтожает матчи, которые были добавлены в соответствующие очереди. 
    /// </summary>
    public class MatchLifeCycleManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchLifeCycleManager));

        private readonly MatchCreator matchCreator;
        private readonly MatchRemover matchRemover;
        
        public MatchLifeCycleManager()
        {
            matchCreator = new MatchCreator();
            matchRemover = new MatchRemover();
        }

        public void UpdateMatchesLifeStatus()
        {
            matchCreator.CreateMatches();
            matchRemover.DeleteFinishedBattles();
        }
    }
}