using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    public class HealthUpdaterSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<ServerGameEntity> playersWithHp;

        public HealthUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            var gameContext = contexts.serverGame;
            playersWithHp = gameContext.GetGroup(ServerGameMatcher
                .AllOf(ServerGameMatcher.Player, ServerGameMatcher.HealthPoints)
                .NoneOf(ServerGameMatcher.Bot));
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