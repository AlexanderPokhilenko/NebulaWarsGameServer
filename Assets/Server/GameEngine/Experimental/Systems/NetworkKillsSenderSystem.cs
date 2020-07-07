using System.Collections.Generic;
using Code.Common;
using Entitas;
using Server.Http;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Отвечает за отправку сообщения о смертях, которые произошли за этот кадр.
    /// </summary>
    public class NetworkKillsSenderSystem : ReactiveSystem<GameEntity>
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(NetworkKillsSenderSystem));
        
        private readonly int matchId;
        private readonly PlayerDeathHandler playerDeathHandler;

        private readonly UdpSendUtils udpSendUtils;
        
        readonly IGroup<GameEntity> alivePlayersAndBots;
        private readonly IGroup<GameEntity> alivePlayers;
        private readonly Dictionary<int, (int playerId, ViewTypeId type)> killersInfo;
    
        public NetworkKillsSenderSystem(Contexts contexts, 
            Dictionary<int, (int playerId, ViewTypeId type)> killersInfos, int matchId, 
            PlayerDeathHandler playerDeathHandler,  UdpSendUtils udpSendUtils)
            : base(contexts.game)
        {
            killersInfo = killersInfos;
            this.matchId = matchId;
            this.playerDeathHandler = playerDeathHandler;
            this.udpSendUtils = udpSendUtils;
            var gameContext = contexts.game;
            alivePlayers = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Account, GameMatcher.Player).NoneOf(GameMatcher.Bot));
            alivePlayersAndBots = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Account, GameMatcher.Player).NoneOf(GameMatcher.KilledBy));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.KilledBy.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasAccount && entity.hasViewType && entity.hasKilledBy;
        }

        protected override void Execute(List<GameEntity> killedEntities)
        {
            int countOfAlivePlayersAndBots = alivePlayersAndBots.count;
            int countOfKilledEntities = killedEntities.Count;
            
            foreach (var alivePlayer in alivePlayers)
            {
                for (var killedEntityIndex = 0; killedEntityIndex < killedEntities.Count; killedEntityIndex++)
                {
                    GameEntity killedEntity = killedEntities[killedEntityIndex];
                    if (!killersInfo.TryGetValue(killedEntity.killedBy.id, out var killerInfo))
                    {
                        killerInfo = (0, 0);
                    }

                    KillData killData = new KillData
                    {
                        TargetPlayerId = alivePlayer.player.id,
                        KillerId = killerInfo.playerId,
                        KillerType = killerInfo.type,
                        VictimType = killedEntity.viewType.id,
                        VictimId = killedEntity.account.id
                    };
                
                    udpSendUtils.SendKill(matchId, killData);

                    if (!killedEntity.isBot)
                    {
                        int playerId = killedEntity.player.id;
                        int placeInBattle = GetPlaceInBattle(countOfAlivePlayersAndBots, countOfKilledEntities,
                            killedEntityIndex);
                        
                        PlayerDeathData playerDeathData = new PlayerDeathData
                        {
                            PlayerId = playerId,
                            PlaceInBattle = placeInBattle,
                            MatchId = matchId 
                        };
                        
                        playerDeathHandler.PlayerDeath(playerDeathData, true);
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
