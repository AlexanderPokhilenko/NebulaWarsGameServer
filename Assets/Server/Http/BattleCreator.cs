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
        
        public GameRoomValidationResult Handle(BattleRoyaleMatchData roomData)
        {
            DebugLogGameRoom(roomData);
            GameRoomValidationResult result = CheckRoom(roomData);
            if (result?.ResultEnum == GameRoomValidationResultEnum.Ok)
            {
                AddRoomToQueue(roomData);
            }
            else
            {
                throw new Exception("От гейм матчера пришло сообщение о создании комнаты, но оно было " +
                                    $"отклонено. result?.ResultEnum = {result?.ResultEnum.ToString()}");
            }
            return result;
        }
        
        private GameRoomValidationResult CheckRoom(BattleRoyaleMatchData roomData)
        {
            bool roomWithThisNumberDoesNotExist = CheckRoomNumber(roomData);
            bool thereIsNoRoomWithSuchPlayers = CheckPlayers(roomData);
            var result = GetValidationResult(roomWithThisNumberDoesNotExist, thereIsNoRoomWithSuchPlayers);
            return result;
        }

        private bool CheckPlayers(BattleRoyaleMatchData matchData)
        {
            if (GameEngineMediator.MatchStorageFacade == null)
            {
                throw new Exception("Игра ещё не инициализирована.");
            }

            bool thereIsNoRoomWithSuchPlayers = true;
            foreach (var playerId in matchData.GameUnitsForMatch.Players.Select(player => player.TemporaryId))
            {
                if (GameEngineMediator.MatchStorageFacade.HasPlayerWithId(playerId))
                {
                    Log.Error("В словаре уже содержится игрок с id = "+playerId);
                    thereIsNoRoomWithSuchPlayers = false;
                    break;
                }
            }
            return thereIsNoRoomWithSuchPlayers;
        }

        private bool CheckRoomNumber(BattleRoyaleMatchData matchData)
        {
            if (GameEngineMediator.MatchStorageFacade == null)
            {
                throw new Exception("Игра ещё не инициализирована.");
            }
            
            return !GameEngineMediator.MatchStorageFacade.HasMatchWithId(matchData.MatchId);
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

        private static void AddRoomToQueue(BattleRoyaleMatchData matchData)
        {
            if (GameEngineMediator.MatchStorageFacade == null)
                throw new Exception("Игра ещё не инициализирована.");
            
            
            GameEngineMediator.MatchStorageFacade.AddMatchToQueue(matchData);
        }

        private static void DebugLogGameRoom(BattleRoyaleMatchData matchData)
        {
            Log.Info("Информация об игрой комнате");
            Log.Info($"{nameof(matchData.MatchId)}  {matchData.MatchId}");
            Log.Info($"{nameof(matchData.GameServerIp)}  {matchData.GameServerIp}");
            Log.Info($"{nameof(matchData.GameServerPort)}  {matchData.GameServerPort}");
            
            Log.Info("Игроки");
            foreach (var player in matchData.GameUnitsForMatch.Players)
            {
                Log.Info($"{nameof(player.ServiceId)} = {player.ServiceId}, {nameof(player.AccountId)} = {player.AccountId}, {player.PrefabName} lvl = {player.WarshipCombatPowerLevel}");
            }
            
            Log.Info("Боты");
            foreach (var bot in matchData.GameUnitsForMatch.Bots)
            {
                Log.Info($"{nameof(bot.TemporaryId)} = {bot.TemporaryId} {bot.PrefabName} lvl = {bot.WarshipCombatPowerLevel}");
            }
        }
    }
}