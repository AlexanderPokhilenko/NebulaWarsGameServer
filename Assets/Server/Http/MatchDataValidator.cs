using System;
using System.Linq;
using Code.Common;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine;

namespace Server.Http
{
    /// <summary>
    /// Проверяет содержимое объекта с информацией про матч, который нужно создать. 
    /// </summary>
    public class MatchDataValidator
    {
        private readonly MatchStorage matchStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchModelMessageHandler));

        public MatchDataValidator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public GameRoomValidationResult Validate(BattleRoyaleMatchModel matchModel)
        {
            CheckPrefabNames(matchModel);
            bool matchWithThisIdDoesNotExist = CheckMatchId(matchModel);
            bool thereIsNoMatchWithSuchPlayers = CheckPlayers(matchModel);
            var result = GetValidationResult(matchWithThisIdDoesNotExist, thereIsNoMatchWithSuchPlayers);
            return result;
        }

        private void CheckPrefabNames(BattleRoyaleMatchModel matchModel)
        {
            for (int i = 0; i < matchModel.GameUnitsForMatch.Count(); i++)
            {
                var gameUnit = matchModel.GameUnitsForMatch[i];
                if (string.IsNullOrWhiteSpace(gameUnit.PrefabName))
                {
                    throw new ArgumentException(nameof(gameUnit.PrefabName));
                }
            }
        }
        
        private bool CheckPlayers(BattleRoyaleMatchModel matchModel)
        {
            bool thereIsNoRoomWithSuchPlayers = true;
            foreach (var playerId in matchModel.GameUnitsForMatch.Players.Select(player => player.TemporaryId))
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

        private bool CheckMatchId(BattleRoyaleMatchModel matchModel)
        {
            return !matchStorage.HasMatch(matchModel.MatchId);
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