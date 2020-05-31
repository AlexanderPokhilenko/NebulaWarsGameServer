using log4net;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    /// <summary>
    /// При смерти игрока или при самостоятельном выходе из боя
    /// 1) Отправляет ему сообщения для показа статистики боя
    /// 2) Удаляет ip игрока, чтобы ему не приходили сообщения
    /// 3) Сообщает о смерти игрока матчмейкеру.
    /// </summary>
    public class PlayerDeathHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PlayerDeathHandler));
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly MatchmakerNotifier matchmakerNotifier;
        private readonly UdpSendUtils udpSendUtils;

        public PlayerDeathHandler(IpAddressesStorage ipAddressesStorage, 
            MatchmakerNotifier matchmakerNotifier, UdpSendUtils udpSendUtils)
        {
            this.ipAddressesStorage = ipAddressesStorage;
            this.matchmakerNotifier = matchmakerNotifier;
            this.udpSendUtils = udpSendUtils;
        }
        
        public void PlayerDeath(PlayerDeathData playerDeathData, bool sendNotificationToPlayer)
        {
            if (sendNotificationToPlayer)
            {
                udpSendUtils.SendShowAchievementsMessage(playerDeathData.MatchId, playerDeathData.PlayerId);   
            }
            
            RemoveFromActivePlayers(playerDeathData);
            SendPlayerDeathMessageToMatchmaker(playerDeathData);
        }
        
        private void RemoveFromActivePlayers(PlayerDeathData playerDeathData)
        {
            bool success = ipAddressesStorage.TryRemoveIpEndPoint(playerDeathData.PlayerId);
            if (!success)
            {
                log.Warn($"Не удалось удалить ip-адрес игрока с " +
                         $"{nameof(playerDeathData.PlayerId)} {playerDeathData.PlayerId} " +
                         $"{nameof(playerDeathData.MatchId)} {playerDeathData.MatchId}");
            }
        }
        
        private void SendPlayerDeathMessageToMatchmaker(PlayerDeathData playerDeathData)
        {
            matchmakerNotifier.MarkPlayerAsDeath(playerDeathData);
        }
    }
}