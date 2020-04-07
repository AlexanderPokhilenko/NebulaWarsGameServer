using System.Collections.Generic;
using log4net;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    public static class PlayersNotifyHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayersNotifyHelper));
        public static void Notify(int matchId, IEnumerable<int> playersIds)
        {
            Log.Warn($"{nameof(Notify)} Старт уведомления игроков про окончание матча");
            foreach (var playerId in playersIds)
            {
                UdpSendUtils.SendBattleFinishMessage(matchId, playerId);
            }
            Log.Warn($"{nameof(Notify)} Конец уведомления игроков про окончание матча");
        }
    }
}