using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class DetachesSenderSystem : ReactiveSystem<GameEntity>
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> playersGroup;

        public DetachesSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base(contexts.game)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            playersGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Parent.Removed());
        }

        protected override bool Filter(GameEntity entity)
        {
            return !entity.hasParent && entity.hasViewType && entity.isLongParent && !entity.isDestroyed;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var detaches = entities.Select(e => e.id.value).ToArray();
            foreach (var playerEntity in playersGroup)
            {
                udpSendUtils.SendDetaches(matchId, playerEntity.player.id, detaches);
            }
        }
    }
}