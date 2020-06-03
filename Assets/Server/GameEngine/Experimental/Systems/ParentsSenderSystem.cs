using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    public class ParentsSenderSystem : ReactiveSystem<GameEntity>
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> playersGroup;

        public ParentsSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base(contexts.game)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            playersGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Parent);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasParent && entity.hasViewType;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var parentsDict = entities.ToDictionary(e => e.id.value, e => e.parent.id);
            foreach (var playerEntity in playersGroup)
            {
                udpSendUtils.SendParents(matchId, playerEntity.player.id, parentsDict);
            }
        }
    }
}