using System.Collections.Generic;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class MaxHpUpdaterSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> playersWithHpGroup;
        
        public MaxHpUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base(contexts.game)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            playersWithHpGroup = contexts.game
                .GetGroup(GameMatcher
                    .AllOf(GameMatcher.Player, GameMatcher.MaxHealthPoints)
                    .NoneOf(GameMatcher.Bot));
        }

        public void Initialize()
        {
            foreach (var gameEntity in playersWithHpGroup)
            {
                udpSendUtils.SendMaxHealthPoints(matchId, gameEntity.player.id, gameEntity.maxHealthPoints.value);
            }
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.MaxHealthPoints);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasPlayer && !entity.isBot && entity.hasMaxHealthPoints;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var gameEntity in entities)
            {
                udpSendUtils.SendMaxHealthPoints(matchId, gameEntity.player.id, gameEntity.maxHealthPoints.value);
            }
        }
    }
}