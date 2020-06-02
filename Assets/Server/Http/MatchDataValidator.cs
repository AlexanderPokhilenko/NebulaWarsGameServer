using System;
using System.Linq;
using Code.Common;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine;

namespace Server.Http
{
    /// <summary>
    /// Проверяет содержимое объекта с информацие про матч, который нужно создать. 
    /// </summary>
    public class MatchDataValidator
    {
        private readonly MatchStorage matchStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchDataMessageHandler));

        public MatchDataValidator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public GameRoomValidationResult Validate(BattleRoyaleMatchData matchData)
        {
            CheckPrefabNames(matchData);
            bool matchWithThisIdDoesNotExist = CheckMatchId(matchData);
            bool thereIsNoMatchWithSuchPlayers = CheckPlayers(matchData);
            var result = GetValidationResult(matchWithThisIdDoesNotExist, thereIsNoMatchWithSuchPlayers);
            return result;
        }

        private void CheckPrefabNames(BattleRoyaleMatchData matchData)
        {
            for (int i = 0; i < matchData.GameUnitsForMatch.Count(); i++)
            {
                var gameUnit = matchData.GameUnitsForMatch[i];
                if (string.IsNullOrWhiteSpace(gameUnit.PrefabName))
                {
                    throw new ArgumentException(nameof(gameUnit.PrefabName));
                }
            }
        }
        
        private bool CheckPlayers(BattleRoyaleMatchData matchData)
        {
            bool thereIsNoRoomWithSuchPlayers = true;
            foreach (var playerId in matchData.GameUnitsForMatch.Players.Select(player => player.TemporaryId))
            {
                if (matchStorage.HasPlayer(playerId))
                {
                    log.Error("В словаре уже содержится игрок с id = "+playerId);
                    thereIsNoRoomWithSuchPlayers = false;
                    break;
                }
            }
            return thereIsNoRoomWithSuchPlayers;
        }

        private bool CheckMatchId(BattleRoyaleMatchData matchData)
        {
            return !matchStorage.HasMatch(matchData.MatchId);
        }

        //TODO говно
        //заменить enum на несколько bool
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
    }
}