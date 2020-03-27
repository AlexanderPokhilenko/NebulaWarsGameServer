using System;
using NetworkLibrary.NetworkLibrary.Http;

namespace Server.Http
{
    public class HttpMessageHandlers
    {
        private readonly BattleCreator battleCreator=new BattleCreator();

        public GameRoomValidationResult Handle(BattleRoyaleMatchData matchData)
        {
            CheckMatchData(matchData);
            return battleCreator.Handle(matchData);
        }

        private void CheckMatchData(BattleRoyaleMatchData matchData)
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
    }
}