using Entitas;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;

namespace Server.GameEngine.Systems
{
    public class MatchIdInitSystem : IInitializeSystem
    {
        private readonly Contexts contexts;
        private readonly BattleRoyaleMatchData matchData;
        
        public MatchIdInitSystem(Contexts contexts, BattleRoyaleMatchData matchData)
        {
            this.contexts = contexts;
            this.matchData = matchData;
        }
        public void Initialize()
        {
            contexts.game.SetMatch(matchData.MatchId);
        }
    }
}