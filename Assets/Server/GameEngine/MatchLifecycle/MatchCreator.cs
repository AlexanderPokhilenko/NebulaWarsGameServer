using System.Collections.Concurrent;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Http;
using SharedSimulationCode;

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
        private readonly MatchFactory matchFactory;
        private readonly ConcurrentQueue<BattleRoyaleMatchModel> matchesToCreate;
        
        public MatchCreator(MatchFactory matchFactory)
        {
            this.matchFactory = matchFactory;
            matchesToCreate = new ConcurrentQueue<BattleRoyaleMatchModel>();
        }
        
        public void AddMatchToCreationQueue(BattleRoyaleMatchModel battleRoyaleMatchModel)
        {
            matchesToCreate.Enqueue(battleRoyaleMatchModel);
        }
        
        public List<MatchSimulation> CreateMatches()
        {
            List<MatchSimulation> result = new List<MatchSimulation>();
            while (!matchesToCreate.IsEmpty)
            {
                if (matchesToCreate.TryDequeue(out BattleRoyaleMatchModel matchModel))
                {
                    MatchSimulation match = matchFactory.Create(matchModel);
                    result.Add(match);
                }
            }

            return result;
        }
    }
}