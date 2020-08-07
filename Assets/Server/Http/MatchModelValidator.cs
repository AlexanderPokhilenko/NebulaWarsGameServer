using System;
using System.Collections.Generic;
using System.Linq;
using Code.Common;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine;
using Server.GameEngine.MatchLifecycle;

namespace Server.Http
{
    /// <summary>
    /// Проверяет содержимое объекта с информацией про матч, который нужно создать. 
    /// </summary>
    public class MatchModelValidator
    {
        //todo переписать это
        private readonly MatchStorage matchStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchModelMessageHandler));

        public MatchModelValidator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public GameRoomValidationResult Validate(BattleRoyaleMatchModel matchModel)
        {
            CheckPrefabNames(matchModel);
            bool matchWithThisIdDoesNotExist = CheckMatchId(matchModel);
            var result = GetValidationResult(matchWithThisIdDoesNotExist);
            return result;
        }

        private void CheckPrefabNames(BattleRoyaleMatchModel matchModel)
        {
            foreach (PlayerModel playerModel in matchModel.GameUnits.Players)
            {
                if (string.IsNullOrWhiteSpace(playerModel.WarshipName))
                {
                    throw new ArgumentException(nameof(playerModel.WarshipName));
                }
            } 
            
            foreach (BotModel botModel in matchModel.GameUnits.Bots)
            {
                if (string.IsNullOrWhiteSpace(botModel.WarshipName))
                {
                    throw new ArgumentException(nameof(botModel.WarshipName));
                }
            }
        }

        private bool CheckMatchId(BattleRoyaleMatchModel matchModel)
        {
            return !matchStorage.HasMatch(matchModel.MatchId);
        }
        
        private static GameRoomValidationResult GetValidationResult(bool roomWithThisNumberDoesNotExist)
        {
            GameRoomValidationResult result = new GameRoomValidationResult();
            if (roomWithThisNumberDoesNotExist)
            {
                result.ResultEnum = GameRoomValidationResultEnum.Ok;
            }
            else
            {
                result.ResultEnum = GameRoomValidationResultEnum.AlreadyHaveARoomWithThatNumber;
            }

            return result;
        }
    }
}