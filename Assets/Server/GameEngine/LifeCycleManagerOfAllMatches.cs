using log4net;

namespace Server.GameEngine
{
    //TODO название не очень
    /// <summary>
    /// Создаёт и уничтожает матчи, которые были добавлены в соответствующие очереди. 
    /// </summary>
    public class LifeCycleManagerOfAllMatches
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LifeCycleManagerOfAllMatches));

        private readonly MatchCreator matchCreator;
        private readonly MatchRemover matchRemover;
        
        public LifeCycleManagerOfAllMatches()
        {
            matchCreator = new MatchCreator();
            matchRemover = new MatchRemover();
        }

        public void UpdateMatchesLifeStatus()
        {
            matchCreator.CreateBattles();
            matchRemover.DeleteFinishedBattles();
        }
    }
}