using System.Collections.Concurrent;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEditor.Experimental.GraphView;

namespace Server.GameEngine
{
    /// <summary>
    /// Правильно создаёт матчи, данные о которых есть в очереди.
    /// </summary>
    public class MatchCreator
    {
        /// <summary>
        /// Очередь на создание.
        /// </summary>
        private readonly ConcurrentQueue<BattleRoyaleMatchData> matchesToCreate;
        private readonly MatchFactory matchFactory;
        
        public MatchCreator(MatchFactory matchFactory)
        {
            this.matchFactory = matchFactory;
            matchesToCreate = new ConcurrentQueue<BattleRoyaleMatchData>();
        }
        
        public void AddMatchToCreationQueue(BattleRoyaleMatchData battleRoyaleMatchData)
        {
            matchesToCreate.Enqueue(battleRoyaleMatchData);
        }
        
        public List<Match> CreateMatches()
        {
            List<Match> result = new List<Match>();
            while (!matchesToCreate.IsEmpty)
            {
                if (matchesToCreate.TryDequeue(out BattleRoyaleMatchData matchData))
                {
                    var match = matchFactory.Create(matchData);
                    result.Add(match);
                }
            }

            return result;
        }
    }
}