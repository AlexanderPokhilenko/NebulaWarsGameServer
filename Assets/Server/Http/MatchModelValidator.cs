using System;
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
            bool thereIsNoMatchWithSuchPlayers = CheckPlayers(matchModel);
            var result = GetValidationResult(matchWithThisIdDoesNotExist, thereIsNoMatchWithSuchPlayers);
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
            } foreach (BotModel botModel in matchModel.GameUnits.Bots)
            {
                if (string.IsNullOrWhiteSpace(botModel.WarshipName))
                {
                    throw new ArgumentException(nameof(botModel.WarshipName));
                }
            }
         
        }
        
        private bool CheckPlayers(BattleRoyaleMatchModel matchModel)
        {
            bool thereIsNoRoomWithSuchPlayers = true;
            var tmpIds = matchModel.GameUnits.Bots.Select(bot => bot.TemporaryId)
                .ToList();
            var test1 = matchModel.GameUnits.Players.Select(player => player.TemporaryId);
            tmpIds.AddRange(test1);
            foreach (ushort tmpId in tmpIds)
            {
                if (matchStorage.HasPlayer(tmpId))
                {
                    log.Error("В словаре уже содержится игрок с id = "+tmpId);
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