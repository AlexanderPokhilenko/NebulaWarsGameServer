using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Http;
using Server.Udp.Sending;


namespace Server.GameEngine
{
    public class BattlesStorage
    {
        private static readonly Lazy<BattlesStorage> Lazy = new Lazy<BattlesStorage> (
            () => new BattlesStorage());
        public static BattlesStorage Instance => Lazy.Value;
        
        //В эту очередь элементы кладутся после получения http от гейм матчера
        public readonly ConcurrentQueue<BattleRoyaleMatchData> battlesToCreate;
        
        //текущие бои
        public readonly ConcurrentDictionary<int, Battle> battles;
        //текущие бои (ключ - id игрока)
        public readonly ConcurrentDictionary<int, Battle> playerToBattle;
        
        //номера боёв, про окончание которых нужно сообщинь гейм матчеру
        private readonly Queue<int> finishedBattles;

        private static readonly ILog Log = LogManager.GetLogger(typeof(BattlesStorage));

        public int GetMatchId(int playerTmpId)
        {
            if (playerToBattle.TryGetValue(playerTmpId, out var battle))
            {
                return battle.matchData.MatchId;
            }
            else
            {
                if (playerToBattle.Count > 0)
                {
                    foreach (var pair in playerToBattle)
                    {
                        Log.Warn($"{pair.Key} {pair.Value.matchData.MatchId}");
                    }
                }
                else
                {
                    Log.Warn($"count is 0");
                }
                throw new Exception($"Не удалось найти matchId {nameof(playerTmpId)} {playerTmpId}");
            }
        }
        public BattlesStorage()
        {
            battlesToCreate = new ConcurrentQueue<BattleRoyaleMatchData>();
            finishedBattles = new Queue<int>();
            battles= new ConcurrentDictionary<int, Battle>();
            playerToBattle = new ConcurrentDictionary<int, Battle>();
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
                if (battlesToCreate.TryDequeue(out var matchData))
                {
                    Battle battle = new Battle(this);
                    battle.ConfigureSystems(matchData);
                    battles.TryAdd(matchData.MatchId, battle);
                    foreach (var player in matchData.GameUnitsForMatch.Players)
                    {
                        Log.Warn($"Добавление игрока {nameof(player.TemporaryId)} {player.TemporaryId}");
                        playerToBattle.TryAdd(player.TemporaryId, battle);
                    }
                    Log.Info("Создана новая комната");
                }
            }
        }
        
        private void DeleteFinishedBattles()
        {
            while (finishedBattles.Count!=0)
            {
                Log.Warn("Удаление боя");
                int battleNumber = finishedBattles.Dequeue();
                Battle battle = battles[battleNumber];
                battle.TearDown();
                int[] playersIds = battle.matchData.GameUnitsForMatch.Players
                    .Select(player => player.TemporaryId).ToArray();
                NotifyPlayers(playersIds);
                ClearPlayers(playersIds);
                battles.TryRemove(battleNumber, out _);
                BattleDeletingNotifier.GameRoomIdsToDelete.Enqueue(battleNumber);
            }
        }

        private void NotifyPlayers(IEnumerable<int> playersIds)
        {
            foreach (var playerId in playersIds)
            {
                UdpSendUtils.SendBattleFinishMessage(playerId);
            }
        }
        
        private void ClearPlayers(IEnumerable<int> playersIds)
        {
            Log.Error(nameof(ClearPlayers));
            foreach (var key in playersIds)
            {
                playerToBattle.TryRemove(key, out _);
            }
        } 
        
        public void MarkBattleAsFinished(int battleNumber)
        {
            finishedBattles.Enqueue(battleNumber);
        }

        public ICollection<Battle> GetAllGameSessions()
        {
            return battles.Values;
        }
    }
}