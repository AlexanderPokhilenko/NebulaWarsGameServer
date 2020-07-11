using System;

namespace Server.Http
{
    public class PlayerDeathMessageValidator
    {
        public void Validate(PlayerDeathData playerDeathData)
        {
            if (playerDeathData.MatchId == default)
            {
                throw new Exception(nameof(playerDeathData.MatchId));
            }
            
            if (playerDeathData.PlayerAccountId == default)
            {
                throw new Exception(nameof(playerDeathData.PlayerAccountId));
            }
            
            if (playerDeathData.PlaceInBattle == default)
            {
                throw new Exception(nameof(playerDeathData.PlaceInBattle));
            }
        }
    }
}