using System;
using Code.Common;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine;

namespace Server.Http
{
    //TODO говно
    public class MatchModelMessageHandler
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchModelMessageHandler));
        
        private readonly MatchDataValidator matchDataValidator;
        private readonly MatchCreator matchCreator;

        public MatchModelMessageHandler(MatchCreator matchCreator, MatchStorage matchStorage)
        {
            this.matchCreator = matchCreator;
            matchDataValidator = new MatchDataValidator(matchStorage);
        }
        
        public GameRoomValidationResult Handle(BattleRoyaleMatchModel matchModel)
        {
            GameRoomValidationResult result = matchDataValidator.Validate(matchModel);
            if (result?.ResultEnum == GameRoomValidationResultEnum.Ok)
            {
                AddMatchToQueue(matchModel);
            }
            else
            {
                string errMessage = "От матчмейкера пришло сообщение о создании матча, но оно было " +
                                    $"отклонено. result?.ResultEnum = {result?.ResultEnum.ToString()}";
                throw new Exception(errMessage);
            }
            return result;
        }

        private void AddMatchToQueue(BattleRoyaleMatchModel matchModel)
        {
            matchCreator.AddMatchToCreationQueue(matchModel);
        }
    }
}