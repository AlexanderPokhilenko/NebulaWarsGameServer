using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    public class HealthUpdaterSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> playersWithHp;

        public HealthUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            var gameContext = contexts.game;
            playersWithHp = gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.Player, GameMatcher.HealthPoints)
                .NoneOf(GameMatcher.Bot));
        }
        
        public void Execute()
        {
            // foreach (var playerWithHp in playersWithHp)
            // {
            //     udpSendUtils.SendHealthPoints(matchId, playerWithHp.player.id, playerWithHp.healthPoints.value);
            // }
        }
    }
}