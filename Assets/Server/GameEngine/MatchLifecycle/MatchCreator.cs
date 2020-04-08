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
        
        public void CreateMatches()
        {
            while (!matchesToCreate.IsEmpty)
            {
                if (matchesToCreate.TryDequeue(out BattleRoyaleMatchData matchData))
                {
                    CreateMatch(matchData);
                }
            }
        }

       
        private void CreateMatch(BattleRoyaleMatchData matchData)
        {
            Match match = new Match(matchStorageFacade, matchData.MatchId);
            match.ConfigureSystems(matchData);
            matches.TryAdd(match.matchData.MatchId, match);
            foreach (var player in match.matchData.GameUnitsForMatch.Players)
            {
                Log.Info($"Добавление игрока к списку активных игроков {nameof(player.TemporaryId)} " +
                         $"{player.TemporaryId}");
                activePlayers.TryAdd(player.TemporaryId, match);
            }
        }
    }
}