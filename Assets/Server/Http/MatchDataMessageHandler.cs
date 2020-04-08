using System;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine;
using UnityEditor;

namespace Server.Http
{
    //TODO говно
    public class MatchDataMessageHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MatchDataMessageHandler));
        
        private readonly MatchDataValidator matchDataValidator;
        private readonly MatchCreator matchCreator;

        public MatchDataMessageHandler(MatchCreator matchCreator, MatchStorage matchStorage)
        {
            this.matchCreator = matchCreator;
            matchDataValidator = new MatchDataValidator(matchStorage);
        }
        
        public GameRoomValidationResult Handle(BattleRoyaleMatchData matchData)
        {
            GameRoomValidationResult result = matchDataValidator.Validate(matchData);
            if (result?.ResultEnum == GameRoomValidationResultEnum.Ok)
            {
                AddMatchToQueue(matchData);
            }
            else
            {
                string errMessage = "От матчмейкера пришло сообщение о создании матча, но оно было " +
                                    $"отклонено. result?.ResultEnum = {result?.ResultEnum.ToString()}";
                throw new Exception(errMessage);
            }
            return result;
        }

        private void AddMatchToQueue(BattleRoyaleMatchData matchData)
        {
            matchCreator.AddMatchToCreationQueue(matchData);
        }
    }
}