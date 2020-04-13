using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    //TODO отправлять сообщения только при изменении
    public class MaxHpUpdaterSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> playersWithHpGroup;
        
        public MaxHpUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            playersWithHpGroup = contexts.game
                .GetGroup(GameMatcher
                    .AllOf(GameMatcher.Player, GameMatcher.MaxHealthPoints)
                    .NoneOf(GameMatcher.Bot));
        }

        public void Execute()
        {
            foreach (var gameEntity in playersWithHpGroup)
            {
                int playerId = gameEntity.player.id;
                udpSendUtils.SendMaxHealthPoints(matchId, playerId, gameEntity.maxHealthPoints.value);
            }    
        }
    }
}