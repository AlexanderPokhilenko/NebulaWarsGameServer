using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AmoebaBattleServer01.Experimental.Http;
using NetworkLibrary.NetworkLibrary.Http;

namespace AmoebaBattleServer01.Experimental.GameEngine
{
    public class GameSessionsStorage
    {
        //В эту очередь элементы кладутся послу получения http от гейм матчера
        public readonly ConcurrentQueue<GameRoomData> RoomsToCreate = new ConcurrentQueue<GameRoomData>();
        //Очередь номеров игровых сессийЮ про которые нужно сообщинь гейм матчеру
        private readonly Queue<int> finishedGameSessions = new Queue<int>();

        //текущие игровые сессии
        public readonly Dictionary<int, GameSession> GameSessions = new Dictionary<int, GameSession>();
        //текущие игровые сессии (ключ - id игрока)
        public readonly Dictionary<int, GameSession> PlayersToSessions = 
            new Dictionary<int, GameSession>();

        public void UpdateGameSessions()
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
                    GameSession gameSession = new GameSession(this);
                    gameSession.ConfigureSystems(gameRoomData);
                    GameSessions.Add(gameRoomData.GameRoomNumber, gameSession);
                    foreach (var player in gameRoomData.Players)
                    {
                        PlayersToSessions.Add(player.TemporaryIdentifier, gameSession);
                    }
                }
            }
        }
        
        private void RemoveFinishedGames()
        {
            while (finishedGameSessions.Count!=0)
            {
                Console.WriteLine("Удаление игровой сессии");
                int gameSessionNumber = finishedGameSessions.Dequeue();
                var gameSession = GameSessions[gameSessionNumber];
                var playersIds = gameSession.RoomData.Players.Select(player => player.TemporaryIdentifier);
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

        public Dictionary<int,GameSession>.ValueCollection GetAllGameSessions()
        {
            return GameSessions.Values;
        }
    }
}