using System.Collections.Generic;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    public class MaxHpUpdaterSystem : ReactiveSystem<ServerGameEntity>, IInitializeSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<ServerGameEntity> playersWithHpGroup;
        
        public MaxHpUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base(contexts.serverGame)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            playersWithHpGroup = contexts.serverGame
                .GetGroup(ServerGameMatcher
                    .AllOf(ServerGameMatcher.Player, ServerGameMatcher.MaxHealthPoints)
                    .NoneOf(ServerGameMatcher.Bot));
        }

        public void Initialize()
        {
            foreach (var serverGameEntity in playersWithHpGroup)
            {
                udpSendUtils.SendMaxHealthPoints(matchId, serverGameEntity.player.playerId, serverGameEntity.maxHealthPoints.value);
            }
        }

        protected override ICollector<ServerGameEntity> GetTrigger(IContext<ServerGameEntity> context)
        {
            return context.CreateCollector(ServerGameMatcher.MaxHealthPoints);
        }

        protected override bool Filter(ServerGameEntity entity)
        {
            return entity.hasPlayer && !entity.isBot && entity.hasMaxHealthPoints;
        }

        protected override void Execute(List<ServerGameEntity> entities)
        {
            foreach (var serverGameEntity in entities)
            {
                udpSendUtils.SendMaxHealthPoints(matchId, serverGameEntity.player.playerId, serverGameEntity.maxHealthPoints.value);
            }
        }
    }
}