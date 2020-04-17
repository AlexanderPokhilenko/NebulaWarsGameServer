using System;

namespace Server.Http
{
    public class PlayerDeathMessageValidator
    {
        public void Validate(PlayerDeathData playerDeathData)
        {
            if (playerDeathData.MatchId == 0)
            {
                throw new Exception(nameof(playerDeathData.MatchId));
            }
            
            if (playerDeathData.PlayerId == 0)
            {
                throw new Exception(nameof(playerDeathData.PlayerId));
            }
            
            if (playerDeathData.PlaceInBattle == 0)
            {
                throw new Exception(nameof(playerDeathData.PlaceInBattle));
            }
        }
    }
}