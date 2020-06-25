using Code.Common;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    /// <summary>
    /// При смерти игрока или при самостоятельном выходе из боя
    /// 1) Отправляет ему сообщения для показа статистики боя
    /// 2) Сообщает о смерти игрока матчмейкеру.
    /// </summary>
    public class PlayerDeathHandler
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(PlayerDeathHandler));
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly MatchmakerNotifier matchmakerNotifier;
        private readonly UdpSendUtils udpSendUtils;

        public PlayerDeathHandler(MatchmakerNotifier matchmakerNotifier, UdpSendUtils udpSendUtils)
        {
            this.matchmakerNotifier = matchmakerNotifier;
            this.udpSendUtils = udpSendUtils;
        }
        
        public void PlayerDeath(PlayerDeathData playerDeathData, bool sendNotificationToPlayer)
        {
            if (sendNotificationToPlayer)
            {
                udpSendUtils.SendShowAchievementsMessage(playerDeathData.MatchId, playerDeathData.PlayerId);   
            }
            
            SendPlayerDeathMessageToMatchmaker(playerDeathData);
        }

        private void SendPlayerDeathMessageToMatchmaker(PlayerDeathData playerDeathData)
        {
            matchmakerNotifier.MarkPlayerAsExcluded(playerDeathData);
        }
    }
}