using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Отправляет пары accountId-entityId всем игрокам при создании игроков.
    /// </summary>
    public class PlayersSendingSystem : ReactiveSystem<ServerGameEntity>
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IGroup<ServerGameEntity> alivePlayers;
        private readonly IGroup<ServerGameEntity> allPlayersGroup;

        public PlayersSendingSystem(int matchId, Contexts contexts, UdpSendUtils udpSendUtils) 
            : base(contexts.serverGame)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            alivePlayers = contexts.serverGame.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
            allPlayersGroup = contexts.serverGame.GetGroup(ServerGameMatcher.Player);
        }
        
        protected override ICollector<ServerGameEntity> GetTrigger(IContext<ServerGameEntity> context)
        {
            return context.CreateCollector(ServerGameMatcher.Player);
        }

        protected override bool Filter(ServerGameEntity entity)
        {
            return entity.hasPlayer;
        }

        protected override void Execute(List<ServerGameEntity> entities)
        {
            var allPlayers =  allPlayersGroup.GetEntities();
            if (allPlayers.Length == 0)
            {
                throw new Exception("Нет игроков");
            }
            
            Dictionary<int, ushort> dictionary = allPlayers
                .ToDictionary(item => item.account.id,
                            item => item.id.value);
            
            
            foreach (var entity in alivePlayers)
            {
                ushort tmpPlayerId = entity.player.id;
                udpSendUtils.SendPlayerInfo(matchId, tmpPlayerId, dictionary);
            }
        }
    }
}