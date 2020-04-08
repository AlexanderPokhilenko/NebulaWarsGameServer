using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Experimental;
using Server.Http;

namespace Server.GameEngine
{
    /// <summary>
    /// Запускает и останавливает матчи.
    /// </summary>
    public class MatchStorageFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorageFacade));
        
        private readonly ConcurrentQueue<BattleRoyaleMatchData> battlesToCreate;
        private readonly ConcurrentQueue<int> finishedBattles;
        private readonly MatchStorage matchStorage;

        public bool ContainsIpEndPoint(int matchId, int playerId)
        {
            return matchStorage.ContainsIpEndPoint(matchId, playerId);
        }
        
        public bool TryAddEndPoint(int matchId, int playerId, IPEndPoint ipEndPoint)
        {
            return matchStorage.TryAddEndPoint(matchId, playerId, ipEndPoint);
        }
        
        public MatchStorageFacade()
        {
            battlesToCreate = new ConcurrentQueue<BattleRoyaleMatchData>();
            finishedBattles = new ConcurrentQueue<int>();
            matchStorage = new MatchStorage(this);
        }
        
        public void AddMatchToQueue(BattleRoyaleMatchData battleRoyaleMatchData)
        {
            battlesToCreate.Enqueue(battleRoyaleMatchData);
        }
        
        public void MarkBattleAsFinished(int battleNumber)
        {
            finishedBattles.Enqueue(battleNumber);
        }

        public void UpdateBattlesList()
        {
            CreateBattles();
            DeleteFinishedBattles();
        }
        
        private void CreateBattles()
        {
            while (!battlesToCreate.IsEmpty)
            {
                if (battlesToCreate.TryDequeue(out BattleRoyaleMatchData matchData))
                {
                   CreateBattle(matchData);
                }
            }
        }

        private void CreateBattle(BattleRoyaleMatchData matchData)
        {
            matchStorage.CreateMatch(matchData);
        }
        
        private void DeleteFinishedBattles()
        {
            while (finishedBattles.Count != 0)
            {
                if (finishedBattles.TryDequeue(out int matchId))
                {
                    Log.Warn("Удаление боя "+matchId);
                    DeleteMatch(matchId);    
                }
            }
        }

        /// <summary>
        /// Удаляет матч из памяти. Уведомляет игроков и матчмейкер о конце матча.
        /// </summary>
        /// <param name="matchId"></param>
        private void DeleteMatch(int matchId)
        {
            Log.Warn($"{nameof(DeleteMatch)} {nameof(matchId)} {matchId}");
            List<int> playersIds = matchStorage.GetActivePlayersIds(matchId);
            PlayersNotifyHelper.Notify(matchId, playersIds);
            matchStorage.TearDownMatch(matchId);
            matchStorage.RemoveMatch(matchId);
            MatchDeletingNotifier.SendMatchDeletingMessage(matchId);
        }
        
        public ICollection<Match> GetAllGameSessions()
        {
            return matchStorage.DichGetMatches();
        }
        
        public bool TryRemovePlayer(int matchId, int playerId)
        {
            bool success = matchStorage.TryRemovePlayer(matchId, playerId);
            return success;
        }

        public bool HasMatchWithId(int matchId)
        {
            return matchStorage.HasMatchWithId(matchId);
        }
        
        public bool HasPlayerWithId(int playerId)
        {
            return matchStorage.HasPlayerWithId(playerId);
        }

        public List<ReliableMessagesPack> GetActivePlayersRudpMessages()
        {
            return matchStorage.GetActivePlayersRudpMessages();
        }

        public bool TryGetMatchByPlayerId(int playerId, out Match match)
        {
            return matchStorage.TryGetMatchByPlayerId(playerId, out match);
        }

        public bool TryGetPlayerIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            return matchStorage.TryGetPlayerIpEndPoint(matchId, playerId, out ipEndPoint);
        }

        public void AddReliableMessage(int matchId, int playerId, uint messageId, byte[] serializedMessage)
        {
            matchStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
        }

        public void RemoveRudpMessage(uint messageIdToConfirm)
        {
            matchStorage.RemoveRudpMessage(messageIdToConfirm);
        }
    }

    
}