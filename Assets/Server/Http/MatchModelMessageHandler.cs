using System;
using Code.Common;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine;
using Server.GameEngine.MatchLifecycle;

namespace Server.Http
{
    //TODO говно
    public class MatchModelMessageHandler
    {
        private readonly MatchCreator matchCreator;
        private readonly MatchModelValidator matchModelValidator;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchModelMessageHandler));
        
        public MatchModelMessageHandler(MatchCreator matchCreator, MatchStorage matchStorage)
        {
            this.matchCreator = matchCreator;
            matchModelValidator = new MatchModelValidator(matchStorage);
        }
        
        public GameRoomValidationResult Handle(BattleRoyaleMatchModel matchModel)
        {
            GameRoomValidationResult result = matchModelValidator.Validate(matchModel);
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