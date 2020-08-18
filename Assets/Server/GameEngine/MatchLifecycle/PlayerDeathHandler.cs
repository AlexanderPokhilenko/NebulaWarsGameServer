using Code.Common;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;
using System.Threading.Tasks;
using Libraries.Logger;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// При смерти игрока или при самостоятельном выходе из боя
    /// 1) Отправляет ему сообщения для показа статистики боя
    /// 2) Сообщает о смерти игрока матчмейкеру.
    /// </summary>
    public class PlayerDeathHandler
    {
        private readonly UdpSendUtils udpSendUtils;
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly MatchmakerNotifier matchmakerNotifier;
        private readonly ILog log = LogManager.CreateLogger(typeof(PlayerDeathHandler));

        public PlayerDeathHandler(MatchmakerNotifier matchmakerNotifier, UdpSendUtils udpSendUtils,
            IpAddressesStorage ipAddressesStorage)
        {
            this.udpSendUtils = udpSendUtils;
            this.matchmakerNotifier = matchmakerNotifier;
            this.ipAddressesStorage = ipAddressesStorage;
        }
        
        public void PlayerDeath(PlayerDeathData playerDeathData, ushort temporaryId, bool sendNotificationToPlayer)
        {
            if (sendNotificationToPlayer)
            {
                udpSendUtils.SendShowAchievementsMessage(playerDeathData.MatchId, temporaryId);   
            }

            Task.Run(async () =>
            {
                //todo вынести это в отдельную систему
                await Task.Delay(10_000);
                ipAddressesStorage.TryRemoveIpEndPoint(playerDeathData.MatchId, temporaryId);
            });
            
            SendPlayerDeathMessageToMatchmaker(playerDeathData);
        }

        private void SendPlayerDeathMessageToMatchmaker(PlayerDeathData playerDeathData)
        {
            matchmakerNotifier.MarkPlayerAsExcluded(playerDeathData);
        }
    }
}