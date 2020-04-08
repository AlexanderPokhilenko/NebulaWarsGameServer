using System.Collections.Generic;
using log4net;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    public static class PlayersNotifyHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayersNotifyHelper));
        
        public static void Notify(int matchId, List<int> playersIds)
        {
            Log.Warn($"{nameof(Notify)} Старт уведомления игроков про окончание матча");
            foreach (int playerId in playersIds)
            {
                Log.Warn($"Отправка уведомления о завуршении боя игроку {nameof(playerId)} {playerId}");
                UdpSendUtils.SendBattleFinishMessage(matchId, playerId);
            }
            Log.Warn($"{nameof(Notify)} Конец уведомления игроков про окончание матча");
        }
    }
}