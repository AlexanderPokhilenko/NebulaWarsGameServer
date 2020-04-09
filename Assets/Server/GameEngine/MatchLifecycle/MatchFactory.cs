using NetworkLibrary.NetworkLibrary.Http;

namespace Server.GameEngine
{
    public class MatchFactory
    {
        private readonly MatchRemover matchRemover;

        public MatchFactory(MatchRemover matchRemover)
        {
            this.matchRemover = matchRemover;
        }
        
        public Match Create(BattleRoyaleMatchData matchData)
        {
            Match match = new Match(matchData.MatchId, matchRemover);
            match.ConfigureSystems(matchData);
            return match;
        }
    }
}