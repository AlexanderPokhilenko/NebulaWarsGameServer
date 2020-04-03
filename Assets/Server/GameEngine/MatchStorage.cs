using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Http;
using Server.Udp.Sending;

//TODO Это пиздец

namespace Server.GameEngine
{
    /// <summary>
    /// Скрывает в себе данные про текущие матчи.
    /// </summary>
    public class MatchStorage
    {
        private static readonly Lazy<MatchStorage> Lazy = new Lazy<MatchStorage> (() => new MatchStorage());
        public static MatchStorage Instance => Lazy.Value;
        
        //В эту очередь элементы кладутся после получения http от гейм матчера
        public readonly ConcurrentQueue<BattleRoyaleMatchData> battlesToCreate;
        
        //текущие бои
        public readonly ConcurrentDictionary<int, Match> matches;
        //текущие бои (ключ - id игрока)
        public readonly ConcurrentDictionary<int, Match> playerToBattle;
        
        //номера боёв, про окончание которых нужно сообщинь гейм матчеру
        private readonly Queue<int> finishedBattles;

        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorage));

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
        
        public MatchStorage()
        {
            battlesToCreate = new ConcurrentQueue<BattleRoyaleMatchData>();
            finishedBattles = new Queue<int>();
            matches= new ConcurrentDictionary<int, Match>();
            playerToBattle = new ConcurrentDictionary<int, Match>();
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
            //TODO добавить чек
            matches.TryAdd(matchData.MatchId, match);
            foreach (var player in matchData.GameUnitsForMatch.Players)
            {
                Log.Warn($"Добавление игрока {nameof(player.TemporaryId)} {player.TemporaryId}");
                playerToBattle.TryAdd(player.TemporaryId, match);
            }
            Log.Info("Создана новая комната");
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

        private void DeleteMatch(int matchId)
        {
            Match match = matches[matchId];
            match.TearDown();
            int[] playersIds = match.matchData.GameUnitsForMatch.Players
                .Select(player => player.TemporaryId).ToArray();
            NotifyPlayers(playersIds);
            ClearPlayers(playersIds);
            matches.TryRemove(matchId, out _);
            BattleDeletingNotifier.MatchIdsToDelete.Enqueue(matchId);
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

        public ICollection<Match> GetAllGameSessions()
        {
            return matches.Values;
        }
    }
}