using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Http;
using Server.Utils;

namespace Server.GameEngine
{
    public class BattlesStorage
    {
        //В эту очередь элементы кладутся послу получения http от гейм матчера
        public readonly ConcurrentQueue<GameRoomData> RoomsToCreate = new ConcurrentQueue<GameRoomData>();
        //Очередь номеров игровых сессий, про которые нужно сообщинь гейм матчеру
        private readonly Queue<int> finishedGameSessions = new Queue<int>();

        //текущие игровые сессии
        public readonly Dictionary<int, Battle> GameSessions = new Dictionary<int, Battle>();
        //текущие игровые сессии (ключ - id игрока)
        public readonly Dictionary<int, Battle> PlayersToSessions = 
            new Dictionary<int, Battle>();

        public void UpdateGameSessionsState()
        {
            TryAddNewGameSessions();
            RemoveFinishedGames();
        }
        
        private void TryAddNewGameSessions()
        {
            while (!RoomsToCreate.IsEmpty)
            {
                if (RoomsToCreate.TryDequeue(out var gameRoomData))
                {
                    Battle battle = new Battle(this);
                    battle.ConfigureSystems(gameRoomData);
                    GameSessions.Add(gameRoomData.GameRoomNumber, battle);
                    foreach (var player in gameRoomData.Players)
                    {
                        PlayersToSessions.Add(player.TemporaryId, battle);
                    }
                    Log.Info("Создана новая комната");
                }
            }
        }
        
        private void RemoveFinishedGames()
        {
            while (finishedGameSessions.Count!=0)
            {
                Log.Info("Удаление игровой сессии");
                int gameSessionNumber = finishedGameSessions.Dequeue();
                var gameSession = GameSessions[gameSessionNumber];
                var playersIds = gameSession.RoomData.Players.Select(player => player.TemporaryId);
                foreach (var playerLogin in playersIds)
                {
                    PlayersToSessions.Remove(playerLogin);
                }
                GameSessions.Remove(gameSessionNumber);
                GameRoomDeletingNotifier.GameRoomIdsToDelete.Enqueue(gameSessionNumber);
            }
        }
        
        public void MarkGameAsFinished(int roomDataGameRoomNumber)
        {
            finishedGameSessions.Enqueue(roomDataGameRoomNumber);
        }

        public Dictionary<int,Battle>.ValueCollection GetAllGameSessions()
        {
            return GameSessions.Values;
        }
    }
}