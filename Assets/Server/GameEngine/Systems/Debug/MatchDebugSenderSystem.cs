using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems.Debug
{
    public class MatchDebugSenderSystem:IExecuteSystem
    {
        private readonly int matchId;
        readonly IGroup<GameEntity> playersGroup;
        
        public MatchDebugSenderSystem(Contexts contexts, int matchId)
        {
            this.matchId = matchId;
            playersGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
        }
        
        public void Execute()
        {
            foreach (var playerGameEntity in playersGroup)
            {
                UdpSendUtils.SendMatchId(matchId, playerGameEntity .player.id);
            }
        }
    }
}