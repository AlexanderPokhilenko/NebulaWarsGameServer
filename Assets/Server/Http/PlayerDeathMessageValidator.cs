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
            
            if (playerDeathData.PlayerTemporaryId == 0)
            {
                throw new Exception(nameof(playerDeathData.PlayerTemporaryId));
            }
            
            if (playerDeathData.PlaceInBattle == 0)
            {
                throw new Exception(nameof(playerDeathData.PlaceInBattle));
            }
        }
    }
}