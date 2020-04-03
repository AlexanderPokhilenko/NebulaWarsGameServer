using System.Collections.Generic;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    public static class PlayersNotifyHelper
    {
        public static void Notify(IEnumerable<int> playersIds)
        {
            foreach (var playerId in playersIds)
            {
                UdpSendUtils.SendBattleFinishMessage(playerId);
            }
        }
    }
}