using System;
using NetworkLibrary.NetworkLibrary.Http;

namespace Server.Http
{
    public class HttpMessageHandlers
    {
        private readonly HttpMatchDataReceiver httpMatchDataReceiver=new HttpMatchDataReceiver();

        public GameRoomValidationResult Handle(BattleRoyaleMatchData matchData)
        {
            CheckMatchData(matchData);
            return httpMatchDataReceiver.Handle(matchData);
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