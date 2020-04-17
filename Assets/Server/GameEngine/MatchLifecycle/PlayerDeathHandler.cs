using log4net;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    //TODO выглядит плохо
    public class PlayerDeathHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PlayerDeathHandler));
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly MatchmakerMatchStatusNotifier matchmakerMatchStatusNotifier;
        private readonly UdpSendUtils udpSendUtils;

        public PlayerDeathHandler(IpAddressesStorage ipAddressesStorage, 
            MatchmakerMatchStatusNotifier matchmakerMatchStatusNotifier, UdpSendUtils udpSendUtils)
        {
            
            this.ipAddressesStorage = ipAddressesStorage;
            this.matchmakerMatchStatusNotifier = matchmakerMatchStatusNotifier;
            this.udpSendUtils = udpSendUtils;
        }
        
        public void PlayerDeath(PlayerDeathData playerDeathData, bool prematureExitFromMatch)
        {
            if (!prematureExitFromMatch)
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
                log.Info($"Не удалось удалить ip-адрес игрока с " +
                         $"{nameof(playerDeathData.PlayerId)} {playerDeathData.PlayerId} " +
                         $"{nameof(playerDeathData.MatchId)} {playerDeathData.MatchId}");
            }
        }
        
        private void SendPlayerDeathMessageToMatchmaker(PlayerDeathData playerDeathData)
        {
            matchmakerMatchStatusNotifier.MarkPlayerAsDeath(playerDeathData);
        }
    }
}