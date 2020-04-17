using NetworkLibrary.NetworkLibrary.Http;

using Server.Http;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    public class MatchFactory
    {
        private readonly MatchRemover matchRemover;
        private readonly UdpSendUtils udpSendUtils;
        private readonly MatchmakerMatchStatusNotifier matchmakerMatchStatusNotifier;

        public MatchFactory(MatchRemover matchRemover, UdpSendUtils udpSendUtils,
            MatchmakerMatchStatusNotifier matchmakerMatchStatusNotifier)
        {
            this.matchRemover = matchRemover;
            this.udpSendUtils = udpSendUtils;
            this.matchmakerMatchStatusNotifier = matchmakerMatchStatusNotifier;
        }
        
        public Match Create(BattleRoyaleMatchData matchData)
        {
            Match match = new Match(matchData.MatchId, matchRemover, matchmakerMatchStatusNotifier);
            match.ConfigureSystems(matchData, udpSendUtils);
            return match;
        }
    }
}