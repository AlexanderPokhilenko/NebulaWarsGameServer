using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Http;
using Server.Udp.Sending;
using Server.Utils;

namespace Server.GameEngine
{
    public class BattlesStorage
    {
        //В эту очередь элементы кладутся послу получения http от гейм матчера
        public readonly ConcurrentQueue<GameRoomData> battlesToCreate;
        
        //текущие бои
        public readonly Dictionary<int, Battle> battles;
        //текущие бои (ключ - id игрока)
        public readonly Dictionary<int, Battle> playerToBattle;
        
        //номера боёв, про окончание которых нужно сообщинь гейм матчеру
        private readonly Queue<int> finishedBattles;

        public BattlesStorage()
        {
            battlesToCreate = new ConcurrentQueue<GameRoomData>();
            finishedBattles = new Queue<int>();
            battles= new Dictionary<int, Battle>();
            playerToBattle = new Dictionary<int, Battle>();
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
                if (battlesToCreate.TryDequeue(out var gameRoomData))
                {
                    Battle battle = new Battle(this);
                    battle.ConfigureSystems(gameRoomData);
                    battles.Add(gameRoomData.GameRoomNumber, battle);
                    foreach (var player in gameRoomData.Players)
                    {
                        playerToBattle.Add(player.TemporaryId, battle);
                    }
                    Log.Info("Создана новая комната");
                }
            }
        }
        
        private void DeleteFinishedBattles()
        {
            while (finishedBattles.Count!=0)
            {
                Log.Warning("Удаление боя");
                int battleNumber = finishedBattles.Dequeue();
                Battle battle = battles[battleNumber];
                int[] playersIds = battle.RoomData.Players.Select(player => player.TemporaryId).ToArray();
                ClearPlayers(playersIds);
                NotifyPlayers(playersIds);
                battles.Remove(battleNumber);
                MetaServerBattleDeletingNotifier.GameRoomIdsToDelete.Enqueue(battleNumber);
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
            foreach (var playerLogin in playersIds)
            {
                playerToBattle.Remove(playerLogin);
            }
        } 
        
        public void MarkBattleAsFinished(int battleNumber)
        {
            finishedBattles.Enqueue(battleNumber);
        }

        public Dictionary<int,Battle>.ValueCollection GetAllGameSessions()
        {
            return battles.Values;
        }
    }
}