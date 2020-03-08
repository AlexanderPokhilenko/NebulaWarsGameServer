using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class MaxHpUpdaterSystem : IExecuteSystem
    {
        readonly IGroup<GameEntity> playersWithHpGroup;
        
        public MaxHpUpdaterSystem(Contexts contexts)
        {
            playersWithHpGroup = contexts
                .game
                .GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.HealthPoints, GameMatcher.MaxHealthPoints));
        }

        public void Execute()
        {
            foreach (var gameEntity in playersWithHpGroup.AsEnumerable())
            {
                int playerId = gameEntity.player.id;
                UdpSendUtils.SendMaxHealthPoints(playerId, gameEntity.maxHealthPoints.value);
            }    
        }
    }
}