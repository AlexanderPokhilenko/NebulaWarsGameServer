using System.Collections.Generic;
using Entitas;
using Plugins.submodules.SharedCode.Logger;
using Server.GameEngine.MatchLifecycle;
using Server.Http;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    /// <summary>
    /// Отвечает за отправку сообщения о смертях, которые произошли за этот кадр.
    /// </summary>
    public class NetworkKillsSenderSystem : ReactiveSystem<ServerGameEntity>
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(NetworkKillsSenderSystem));
        
        private readonly int matchId;
        private readonly PlayerDeathHandler playerDeathHandler;

        private readonly UdpSendUtils udpSendUtils;
        
        readonly IGroup<ServerGameEntity> alivePlayersAndBots;
        private readonly IGroup<ServerGameEntity> alivePlayers;
        private readonly Dictionary<int, (int playerId, ViewTypeEnum type)> killersInfo;
    
        public NetworkKillsSenderSystem(Contexts contexts, 
            Dictionary<int, (int playerId, ViewTypeEnum type)> killersInfos, int matchId, 
            PlayerDeathHandler playerDeathHandler,  UdpSendUtils udpSendUtils)
            : base(contexts.serverGame)
        {
            killersInfo = killersInfos;
            this.matchId = matchId;
            this.playerDeathHandler = playerDeathHandler;
            this.udpSendUtils = udpSendUtils;
            var gameContext = contexts.serverGame;
            alivePlayers = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Account, ServerGameMatcher.Player).NoneOf(ServerGameMatcher.Bot));
            alivePlayersAndBots = gameContext.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Account, ServerGameMatcher.Player).NoneOf(ServerGameMatcher.KilledBy));
        }

        protected override ICollector<ServerGameEntity> GetTrigger(IContext<ServerGameEntity> context)
        {
            return context.CreateCollector(ServerGameMatcher.KilledBy.Added());
        }

        protected override bool Filter(ServerGameEntity entity)
        {
            return entity.hasAccount && entity.hasViewType && entity.hasKilledBy;
        }

        protected override void Execute(List<ServerGameEntity> killedEntities)
        {
            int countOfAlivePlayersAndBots = alivePlayersAndBots.count;
            int countOfKilledEntities = killedEntities.Count;
            
            foreach (var alivePlayer in alivePlayers)
            {
                for (var killedEntityIndex = 0; killedEntityIndex < killedEntities.Count; killedEntityIndex++)
                {
                    ServerGameEntity killedEntity = killedEntities[killedEntityIndex];
                    if (!killersInfo.TryGetValue(killedEntity.killedBy.id, out var killerInfo))
                    {
                        killerInfo = (0, 0);
                    }

                    KillData killData = new KillData
                    {
                        TargetPlayerTmpId = alivePlayer.player.id,
                        KillerId = killerInfo.playerId,
                        KillerType = killerInfo.type,
                        VictimType = killedEntity.viewType.value,
                        VictimId = killedEntity.account.id
                    };
                
                    udpSendUtils.SendKill(matchId, killData);

                    if (!killedEntity.isBot)
                    {
                        var temporaryId = killedEntity.player.id;
                        var accountId = killedEntity.account.id;
                        var placeInBattle = GetPlaceInBattle(countOfAlivePlayersAndBots, countOfKilledEntities,
                            killedEntityIndex);
                        
                        PlayerDeathData playerDeathData = new PlayerDeathData
                        {
                            PlayerAccountId = accountId,
                            PlaceInBattle = placeInBattle,
                            MatchId = matchId 
                        };
                        
                        playerDeathHandler.PlayerDeath(playerDeathData, temporaryId, true);
                    }
                }
            }
        }

        /// <summary>
        /// Если за один кадр умерло больше одного игрока, то им выдадутся разные места в бою.
        /// </summary>
        private int GetPlaceInBattle(int countOfAlivePlayersAndBots, int countOfKilledEntities, int killedEntityIndex)
        {
            return countOfAlivePlayersAndBots + 1 + (countOfKilledEntities - 1 - killedEntityIndex);
        }
    }
}
