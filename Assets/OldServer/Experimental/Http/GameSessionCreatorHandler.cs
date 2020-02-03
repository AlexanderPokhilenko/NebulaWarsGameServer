using System;
using System.Linq;
using AmoebaBattleServer01.Experimental.GameEngine;
using NetworkLibrary.NetworkLibrary.Http;

namespace AmoebaBattleServer01.Experimental.Http
{
    /// <summary>
    /// Добавляет комнаты в очередь на создание.
    /// </summary>
    internal class GameSessionCreatorHandler
    {
        public GameRoomValidationResult Handle(GameRoomData roomData)
        {
            GameRoomValidationResult result = CheckRoom(roomData);
            if (result?.ResultEnum == GameRoomValidationResultEnum.Ok)
            {
                AddRoomToQueue(roomData);
            }
            else
            {
                throw new Exception($"От гейм матчера пришло сообщение о создании комнаты, но оно было " +
                                    $"отклонено. result?.ResultEnum = {result?.ResultEnum.ToString()}");
            }
            return result;
        }
        
        private GameRoomValidationResult CheckRoom(GameRoomData roomData)
        {
            DebugLogGameRoom(roomData);
            
            bool roomWithThisNumberDoesNotExist = CheckRoomNumber(roomData);
            bool thereIsNoRoomWithSuchPlayers = CheckPlayers(roomData);
            var result = GetValidationResult(roomWithThisNumberDoesNotExist, thereIsNoRoomWithSuchPlayers);
            return result;
        }

        private bool CheckPlayers(GameRoomData roomData)
        {
            if (GameEngineMediator.GameSessionsStorage == null)
                throw new Exception("Игра ещё не инициализирована.");

            bool thereIsNoRoomWithSuchPlayers = true;
            foreach (var playerId in roomData.Players.Select(player => player.TemporaryIdentifier))
            {
                if (GameEngineMediator.GameSessionsStorage.PlayersToSessions.ContainsKey(playerId))
                {
                    thereIsNoRoomWithSuchPlayers = false;
                    break;
                }
            }
            return thereIsNoRoomWithSuchPlayers;
        }

        private bool CheckRoomNumber(GameRoomData roomData)
        {
            if (GameEngineMediator.GameSessionsStorage == null)
                throw new Exception("Игра ещё не инициализирована.");
            
            return !GameEngineMediator.GameSessionsStorage.GameSessions.ContainsKey(roomData.GameRoomNumber);
        }

        private static GameRoomValidationResult GetValidationResult(bool roomWithThisNumberDoesNotExist,
            bool thereIsNoRoomWithSuchPlayers )
        {
            GameRoomValidationResult result = new GameRoomValidationResult();    
            
            if (roomWithThisNumberDoesNotExist && thereIsNoRoomWithSuchPlayers)
            {
                result.ResultEnum = GameRoomValidationResultEnum.Ok;
            }
            else if(!roomWithThisNumberDoesNotExist)
            {
                result.ResultEnum = GameRoomValidationResultEnum.AlreadyHaveARoomWithThatNumber;
            }else
            {
                result.ResultEnum = GameRoomValidationResultEnum.ThereIsAtLeastOneSuchPlayer;
            }
            
            return result;
        }

        private static void AddRoomToQueue(GameRoomData roomData)
        {
            if (GameEngineMediator.GameSessionsStorage == null)
                throw new Exception("Игра ещё не инициализирована.");
            
            
            GameEngineMediator.GameSessionsStorage.RoomsToCreate.Enqueue(roomData);
        }

        private static void DebugLogGameRoom(GameRoomData roomData)
        {
            Console.WriteLine("Информация об игрой комнате");
            Console.WriteLine($"{nameof(roomData.GameRoomNumber)}  {roomData.GameRoomNumber}");
            Console.WriteLine($"{nameof(roomData.GameServerIp)}  {roomData.GameServerIp}");
            Console.WriteLine($"{nameof(roomData.GameServerPort)}  {roomData.GameServerPort}");
            Console.WriteLine("Игроки");
            foreach (var player in roomData.Players)
            {
                Console.WriteLine($"PlayerGoogleId = {player.GoogleId}");
            }
        }
    }
}