using Entitas;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;

namespace Server.GameEngine.Systems
{
    public class MatchIdInitSystem : IInitializeSystem
    {
        private readonly Contexts contexts;
        private readonly BattleRoyaleMatchData matchData;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchIdInitSystem));
        
        public MatchIdInitSystem(Contexts contexts, BattleRoyaleMatchData matchData)
        {
            this.contexts = contexts;
            this.matchData = matchData;
        }
        public void Initialize()
        {
            Log.Info($"Инициализация {nameof(matchData.MatchId)} {matchData.MatchId}");
            contexts.game.SetMatch(matchData.MatchId);
        }
    }
}