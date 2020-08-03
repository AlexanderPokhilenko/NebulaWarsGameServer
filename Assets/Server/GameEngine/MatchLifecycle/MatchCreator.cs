using System.Collections.Concurrent;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Http;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// Правильно создаёт матчи, данные о которых есть в очереди.
    /// </summary>
    public class MatchCreator
    {
        /// <summary>
        /// Очередь на создание.
        /// </summary>
        private readonly ConcurrentQueue<BattleRoyaleMatchModel> matchesToCreate;
        private readonly MatchFactory matchFactory;
        
        public MatchCreator(MatchFactory matchFactory)
        {
            this.matchFactory = matchFactory;
            matchesToCreate = new ConcurrentQueue<BattleRoyaleMatchModel>();
        }
        
        public void AddMatchToCreationQueue(BattleRoyaleMatchModel battleRoyaleMatchModel)
        {
            matchesToCreate.Enqueue(battleRoyaleMatchModel);
        }
        
        public List<Match> CreateMatches()
        {
            List<Match> result = new List<Match>();
            while (!matchesToCreate.IsEmpty)
            {
                if (matchesToCreate.TryDequeue(out BattleRoyaleMatchModel matchModel))
                {
                    Match match = matchFactory.Create(matchModel);
                    result.Add(match);
                }
            }

            return result;
        }
    }
}