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
        private readonly MessagesPackIdFactory messagesPackIdFactory;

        public MatchFactory(MatchRemover matchRemover, UdpSendUtils udpSendUtils,
            MatchmakerNotifier matchmakerNotifier, IpAddressesStorage ipAddressesStorage,
            MessageIdFactory messageIdFactory, MessagesPackIdFactory messagesPackIdFactory)
        {
            this.matchRemover = matchRemover;
            this.udpSendUtils = udpSendUtils;
            this.messageIdFactory = messageIdFactory;
            this.messagesPackIdFactory = messagesPackIdFactory;
            this.matchmakerNotifier = matchmakerNotifier;
            this.ipAddressesStorage = ipAddressesStorage;
        }
        
        public Match Create(BattleRoyaleMatchModel matchModel)
        {
            ipAddressesStorage.AddMatch(matchModel);

            foreach (ushort playerId in matchModel.GameUnits.Players.Select(player=>player.TemporaryId))
            {
                messageIdFactory.AddPlayer(matchModel.MatchId, playerId);
                messagesPackIdFactory.AddPlayer(matchModel.MatchId, playerId);
            }
            
            Match match = new Match(matchModel.MatchId, matchRemover, matchmakerNotifier);
            match.ConfigureSystems(matchModel, udpSendUtils, ipAddressesStorage);
            return match;
        }
    }
}