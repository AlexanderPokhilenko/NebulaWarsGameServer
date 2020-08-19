using System;
using NetworkLibrary.NetworkLibrary.Http;
using Plugins.submodules.SharedCode.Logger;
using Server.GameEngine.MatchLifecycle;

namespace Server.Http
{
    //TODO говно
    public class MatchModelMessageHandler
    {
        private readonly MatchCreator matchCreator;
        private readonly MatchModelValidator matchModelValidator;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchModelMessageHandler));
        
        public MatchModelMessageHandler(MatchCreator matchCreator, MatchesStorage matchesStorage)
        {
            this.matchCreator = matchCreator;
            matchModelValidator = new MatchModelValidator(matchesStorage);
        }
        
        public GameRoomValidationResult Handle(BattleRoyaleMatchModel matchModel)
        {
            GameRoomValidationResult result = matchModelValidator.Validate(matchModel);
            if (result?.ResultEnum == GameRoomValidationResultEnum.Ok)
            {
                matchCreator.AddMatchToCreationQueue(matchModel);
            }
            else
            {
                string errMessage = "От матчмейкера пришло сообщение о создании матча, но оно было " +
                                    $"отклонено. result?.ResultEnum = {result?.ResultEnum.ToString()}";
                throw new Exception(errMessage);
            }
            return result;
        }
    }
}