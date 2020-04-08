using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class MaxHpUpdaterSystem : IExecuteSystem
    {
        private readonly int matchId;
        readonly IGroup<GameEntity> playersWithHpGroup;
        
        public MaxHpUpdaterSystem(Contexts contexts, int matchId)
        {
            this.matchId = matchId;
            playersWithHpGroup = contexts.game
                .GetGroup(GameMatcher
                    .AllOf(GameMatcher.Player, GameMatcher.HealthPoints, GameMatcher.MaxHealthPoints)
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