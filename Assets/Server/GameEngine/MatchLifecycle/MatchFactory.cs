using System.Linq;
using NetworkLibrary.NetworkLibrary.Http;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine.MatchLifecycle
{
    public class MatchFactory
    {
        private readonly MatchRemover matchRemover;
        private readonly UdpSendUtils udpSendUtils;
        private readonly MessageIdFactory messageIdFactory;
        private readonly MatchmakerNotifier matchmakerNotifier;
        private readonly IpAddressesStorage ipAddressesStorage;

        public MatchFactory(MatchRemover matchRemover, UdpSendUtils udpSendUtils,
            MatchmakerNotifier matchmakerNotifier, IpAddressesStorage ipAddressesStorage,
            MessageIdFactory messageIdFactory)
        {
            this.messageIdFactory = messageIdFactory;
            this.matchRemover = matchRemover;
            this.udpSendUtils = udpSendUtils;
            this.matchmakerNotifier = matchmakerNotifier;
            this.ipAddressesStorage = ipAddressesStorage;
        }
        
        public Match Create(BattleRoyaleMatchModel matchModel)
        {
            ipAddressesStorage.AddMatch(matchModel);

            foreach (ushort playerId in matchModel.GameUnits.Players.Select(player=>player.TemporaryId))
            {
                messageIdFactory.AddPlayer(playerId);
            }
            
            Match match = new Match(matchModel.MatchId, matchRemover, matchmakerNotifier);
            match.ConfigureSystems(matchModel, udpSendUtils, ipAddressesStorage);
            return match;
        }
    }
}