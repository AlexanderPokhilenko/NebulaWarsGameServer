using System.Collections.Generic;
using log4net;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    /// <summary>
    /// Отправляет игрокам сообщения об окончании матча по udp.
    /// </summary>
    public class PlayersMatchFinishNotifier
    {
        private readonly UdpSendUtils udpSendUtils;
        private readonly ILog log = LogManager.GetLogger(typeof(PlayersMatchFinishNotifier));

        public PlayersMatchFinishNotifier(UdpSendUtils udpSendUtils)
        {
            this.udpSendUtils = udpSendUtils;
        }
        
        public void Notify(Match match)
        {
            List<int> activePlayersIds = match.GetActivePlayersIds();
            if (activePlayersIds.Count == 0)
            {
                log.Error("Список активных игроков пуст. Некого уведомлять о окончании матча.");
            }
            else
            {
                log.Info(" Старт уведомления игроков про окончание матча");
                foreach (int playerId in activePlayersIds)
                {
                    log.Info($"Отправка уведомления о завершении боя игроку {nameof(playerId)} {playerId}");
                    udpSendUtils.SendShowAchievementsMessage(match.MatchId, playerId);
                }
                log.Info(" Конец уведомления игроков про окончание матча");
            }
        }
    }
}