﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Experimental;
using Server.Http;
using UnityEngine.Rendering;

namespace Server.GameEngine
{
    /// <summary>
    /// Запускает и останавливает матчи.
    /// </summary>
    public class MatchStorageFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorageFacade));
        
        private readonly ConcurrentQueue<BattleRoyaleMatchData> battlesToCreate;
        private readonly Queue<int> finishedBattles;
        private readonly MatchStorage matchStorage;

        public bool ContainsIpEndPoint(int matchId, int playerId)
        {
            return matchStorage.ContainsIpEndPoint(matchId, playerId);
        }
        
        public void AddEndPoint(int matchId, int playerId, IPEndPoint ipEndPoint)
        {
            matchStorage.AddEndPoint(matchId, playerId, ipEndPoint);
        }
        
        public MatchStorageFacade()
        {
            battlesToCreate = new ConcurrentQueue<BattleRoyaleMatchData>();
            finishedBattles = new Queue<int>();
            matchStorage = new MatchStorage();
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
            Match match = new Match(this);
            match.ConfigureSystems(matchData);
            matchStorage.AddMatch(match);
        }
        
        private void DeleteFinishedBattles()
        {
            while (finishedBattles.Count!=0)
            {
                Log.Warn("Удаление боя");
                int matchId = finishedBattles.Dequeue();
                DeleteMatch(matchId);
            }
        }

        /// <summary>
        /// Удаляет матч из памяти. Уведомляет игроков и матчмейкер о конце матча.
        /// </summary>
        /// <param name="matchId"></param>
        private void DeleteMatch(int matchId)
        {
            Log.Warn("method "+nameof(DeleteMatch));
            var playersIds = matchStorage.GetPlayersIds(matchId);
            matchStorage.TearDownMatch(matchId);
            matchStorage.RemoveMatch(matchId);
            PlayersNotifyHelper.Notify(matchId, playersIds);
            MatchDeletingNotifier.SendMatchDeletingMessage(matchId);
        }
        
        public ICollection<Match> GetAllGameSessions()
        {
            return matchStorage.DichGetMatches();
        }

        public bool TryRemovePlayer(int playerTmpId)
        {
            bool success = matchStorage.TryRemovePlayer(playerTmpId); 
            Log.Info("Удаление игрока из комнаты. success = "+success);
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