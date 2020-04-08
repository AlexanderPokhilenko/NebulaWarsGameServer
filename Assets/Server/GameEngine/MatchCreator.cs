using System.Collections.Concurrent;
using NetworkLibrary.NetworkLibrary.Http;

namespace Server.GameEngine
{
    /// <summary>
    /// Правильно создаёт матчи, которые кладутся в очередь.
    /// </summary>
    public class MatchCreator
    {
        /// <summary>
        /// Очередь на создание.
        /// </summary>
        private readonly ConcurrentQueue<BattleRoyaleMatchData> matchesToCreate;

        public MatchCreator()
        {
            matchesToCreate = new ConcurrentQueue<BattleRoyaleMatchData>();
        }
        
        public void AddMatchToCreationQueue(BattleRoyaleMatchData battleRoyaleMatchData)
        {
            matchesToCreate.Enqueue(battleRoyaleMatchData);
        }
        
        public void CreateBattles()
        {
            while (!matchesToCreate.IsEmpty)
            {
                if (matchesToCreate.TryDequeue(out BattleRoyaleMatchData matchData))
                {
                    CreateBattle(matchData);
                }
            }
        }

        private void CreateBattle(BattleRoyaleMatchData matchData)
        {
            matchStorage.CreateMatch(matchData);
        }
    }
}