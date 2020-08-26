using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    public class DestroysSenderSystem : ReactiveSystem<ServerGameEntity>
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<ServerGameEntity> playersGroup;

        public DestroysSenderSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base(contexts.serverGame)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            playersGroup = contexts.serverGame.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
        }

        protected override ICollector<ServerGameEntity> GetTrigger(IContext<ServerGameEntity> context)
        {
            return context.CreateCollector(ServerGameMatcher.Destroyed.Added());
        }

        protected override bool Filter(ServerGameEntity entity)
        {
            return entity.isDestroyed && entity.hasViewType;
        }

        protected override void Execute(List<ServerGameEntity> entities)
        {
            var destroys = entities.Select(e => e.id.value).ToArray();
            foreach (var playerEntity in playersGroup)
            {
                udpSendUtils.SendDestroys(matchId, playerEntity.player.tmpPlayerId, destroys);
            }
        }
    }
}