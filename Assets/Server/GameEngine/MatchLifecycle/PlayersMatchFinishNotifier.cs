using System.Collections.Generic;
using Plugins.submodules.SharedCode.Logger;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// Отправляет игрокам сообщения об окончании матча по udp.
    /// </summary>
    public class PlayersMatchFinishNotifier
    {
        private readonly UdpSendUtils udpSendUtils;
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(PlayersMatchFinishNotifier));

        public PlayersMatchFinishNotifier(UdpSendUtils udpSendUtils, IpAddressesStorage ipAddressesStorage)
        {
            this.udpSendUtils = udpSendUtils;
            this.ipAddressesStorage = ipAddressesStorage;
        }
        
        public void Notify(ServerMatchSimulation serverMatch)
        {
            List<ushort> activePlayersIds = ipAddressesStorage.GetActivePlayersIds(serverMatch.matchId);
            if (activePlayersIds == null || activePlayersIds.Count == 0)
            {
                log.Info("Список активных игроков пуст. Некого уведомлять о окончании матча.");
            }
            else
            {
                log.Info(" Старт уведомления игроков про окончание матча");
                foreach (ushort playerId in activePlayersIds)
                {
                    log.Info($"Отправка уведомления о завершении боя игроку {nameof(playerId)} {playerId}");
                    udpSendUtils.SendShowAchievementsMessage(serverMatch.matchId, playerId);
                }
                log.Info(" Конец уведомления игроков про окончание матча");
            }
        }
    }
}