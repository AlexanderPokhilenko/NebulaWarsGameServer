using NetworkLibrary.NetworkLibrary.Http;

using Server.Http;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    public class MatchFactory
    {
        private readonly MatchRemover matchRemover;
        private readonly UdpSendUtils udpSendUtils;
        private readonly MatchmakerNotifier matchmakerNotifier;

        public MatchFactory(MatchRemover matchRemover, UdpSendUtils udpSendUtils,
            MatchmakerNotifier matchmakerNotifier)
        {
            this.matchRemover = matchRemover;
            this.udpSendUtils = udpSendUtils;
            this.matchmakerNotifier = matchmakerNotifier;
        }
        
        public Match Create(BattleRoyaleMatchData matchData)
        {
            Match match = new Match(matchData.MatchId, matchRemover, matchmakerNotifier);
            match.ConfigureSystems(matchData, udpSendUtils);
            return match;
        }
    }
}