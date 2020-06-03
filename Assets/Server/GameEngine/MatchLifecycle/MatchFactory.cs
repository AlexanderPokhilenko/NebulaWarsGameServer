using NetworkLibrary.NetworkLibrary.Http;

using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    public class MatchFactory
    {
        //TODO говно
        private readonly MatchRemover matchRemover;
        private readonly UdpSendUtils udpSendUtils;
        private readonly MatchmakerNotifier matchmakerNotifier;
        private readonly IpAddressesStorage ipAddressesStorage;

        public MatchFactory(MatchRemover matchRemover, UdpSendUtils udpSendUtils,
            MatchmakerNotifier matchmakerNotifier, IpAddressesStorage ipAddressesStorage)
        {
            this.matchRemover = matchRemover;
            this.udpSendUtils = udpSendUtils;
            this.matchmakerNotifier = matchmakerNotifier;
            this.ipAddressesStorage = ipAddressesStorage;
        }
        
        public Match Create(BattleRoyaleMatchData matchData)
        {
            ipAddressesStorage.AddMatch(matchData);
            Match match = new Match(matchData.MatchId, matchRemover, matchmakerNotifier);
            match.ConfigureSystems(matchData, udpSendUtils);
            return match;
        }
    }
}