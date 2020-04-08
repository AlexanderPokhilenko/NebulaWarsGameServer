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
        private readonly MatchFactory matchFactory;

        /// <summary>
        /// Очередь на создание.
        /// </summary>
        private readonly ConcurrentQueue<BattleRoyaleMatchData> matchesToCreate;

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

    public class MatchFactory
    {
        private readonly MatchRemover matchRemover;

        public MatchFactory(MatchRemover matchRemover)
        {
            this.matchRemover = matchRemover;
        }
        public Match Create(BattleRoyaleMatchData matchData)
        {
            Match match = new Match(matchData.MatchId, matchRemover);
            match.ConfigureSystems(matchData);
            return match;
        }
    }
}