using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace SharedSimulationCode
{
    public class PlayersSendingSystem : ReactiveSystem<GameEntity>
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<GameEntity> alivePlayers;
        private readonly IGroup<GameEntity> allPlayersGroup;

        public PlayersSendingSystem(int matchId, Contexts contexts, UdpSendUtils udpSendUtils) 
            : base(contexts.game)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            alivePlayers = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            allPlayersGroup = contexts.game.GetGroup(GameMatcher.Player);
        }
        
        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Player);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasPlayer;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var allPlayers =  allPlayersGroup.GetEntities();
            Dictionary<int, ushort> dictionary = allPlayers
                .ToDictionary(item => item.account.id,
                            item => item.id.value);
            
            foreach (var entity in alivePlayers)
            {
                udpSendUtils.SendPlayerInfo(matchId, entity.player.id, dictionary);
            }
        }
    }
}