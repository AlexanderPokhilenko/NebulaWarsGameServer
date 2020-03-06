using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class MaxHpUpdaterSystem:IExecuteSystem
    {
        readonly IGroup<GameEntity> playersWithHpGroup;
        
        public MaxHpUpdaterSystem(Contexts contexts)
        {
            playersWithHpGroup = contexts
                .game
                .GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.HealthPoints));
        }

        public void Execute()
        {
            foreach (var gameEntity in playersWithHpGroup.AsEnumerable())
            {
                int playerId = gameEntity.player.id;
                #warning Сделать нормальное задавание максимального уровня прочности
                int stubMaxHp = 150;
                UdpSendUtils.SendMaxHealthPoints(playerId, stubMaxHp);
            }    
        }
    }
}