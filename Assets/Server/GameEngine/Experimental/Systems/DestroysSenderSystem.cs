using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class DestroysSenderSystem : ReactiveSystem<GameEntity>
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> playersGroup;

        public DestroysSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base(contexts.game)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            playersGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Destroyed.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isDestroyed && entity.hasViewType;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var destroys = entities.Select(e => e.id.value).ToArray();
            foreach (var playerEntity in playersGroup)
            {
                udpSendUtils.SendDestroys(matchId, playerEntity.player.id, destroys);
            }
        }
    }
}