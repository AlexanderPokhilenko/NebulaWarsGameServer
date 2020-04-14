using NetworkLibrary.NetworkLibrary.Http;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    public class MatchFactory
    {
        private readonly MatchRemover matchRemover;
        private readonly UdpSendUtils udpSendUtils;

        public MatchFactory(MatchRemover matchRemover, UdpSendUtils udpSendUtils)
        {
            this.matchRemover = matchRemover;
            this.udpSendUtils = udpSendUtils;
        }
        
        public Match Create(BattleRoyaleMatchData matchData)
        {
            Match match = new Match(matchData.MatchId, matchRemover);
            match.ConfigureSystems(matchData, udpSendUtils);
            return match;
        }
    }
}