using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    //TODO отправлять сообщения только при изменении
    public class MaxHpUpdaterSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly IGroup<GameEntity> playersWithHpGroup;
        
        public MaxHpUpdaterSystem(Contexts contexts, int matchId)
        {
            this.matchId = matchId;
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
                UdpSendUtils.SendMaxHealthPoints(matchId, playerId, gameEntity.maxHealthPoints.value);
            }    
        }
    }
}