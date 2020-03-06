using System;
using System.Linq;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine;


namespace Server.Http
{
    /// <summary>
    /// Добавляет комнаты в очередь на создание.
    /// </summary>
    internal class BattleCreator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BattleCreator));
        
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
            if (GameEngineMediator.BattlesStorage == null)
                throw new Exception("Игра ещё не инициализирована.");

            bool thereIsNoRoomWithSuchPlayers = true;
            foreach (var playerId in roomData.Players.Select(player => player.TemporaryId))
            {
                if (GameEngineMediator.BattlesStorage.playerToBattle.ContainsKey(playerId))
                {
                    thereIsNoRoomWithSuchPlayers = false;
                    break;
                }
            }
            return thereIsNoRoomWithSuchPlayers;
        }

        private bool CheckRoomNumber(GameRoomData roomData)
        {
            if (GameEngineMediator.BattlesStorage == null)
                throw new Exception("Игра ещё не инициализирована.");
            
            return !GameEngineMediator.BattlesStorage.battles.ContainsKey(roomData.GameRoomNumber);
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
            if (GameEngineMediator.BattlesStorage == null)
                throw new Exception("Игра ещё не инициализирована.");
            
            
            GameEngineMediator.BattlesStorage.battlesToCreate.Enqueue(roomData);
        }

        private static void DebugLogGameRoom(GameRoomData roomData)
        {
            Log.Info("Информация об игрой комнате");
            Log.Info($"{nameof(roomData.GameRoomNumber)}  {roomData.GameRoomNumber}");
            Log.Info($"{nameof(roomData.GameServerIp)}  {roomData.GameServerIp}");
            Log.Info($"{nameof(roomData.GameServerPort)}  {roomData.GameServerPort}");
            Log.Info("Игроки");
            foreach (var player in roomData.Players)
            {
                Log.Info($"PlayerGoogleId = {player.GoogleId}");
            }
        }
    }
}