using System;
using System.Collections.Generic;
using Entitas;
using log4net;
using Server.Http;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Отвечает за отправку сообщения об убийствах.
    /// </summary>
    public class NetworkKillsSenderSystem : ReactiveSystem<GameEntity>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NetworkKillsSenderSystem));
        
        private readonly int matchId;
        private readonly Match match;
        private readonly GameContext gameContext;
        readonly IGroup<GameEntity> alivePlayersAndBots;
        private readonly IGroup<GameEntity> alivePlayers;
        private readonly Dictionary<int, (int playerId, ViewTypeId type)> killersInfo;
    
        public NetworkKillsSenderSystem(Contexts contexts, Dictionary<int, (int playerId, ViewTypeId type)> killersInfos, 
            int matchId, Match match)
            : base(contexts.game)
        {
            killersInfo = killersInfos;
            this.matchId = matchId;
            this.match = match;
            gameContext = contexts.game;
            alivePlayers = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            alivePlayersAndBots = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.KilledBy));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.KilledBy.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasPlayer && entity.hasViewType && entity.hasKilledBy;
        }

        protected override void Execute(List<GameEntity> killedEntities)
        {
            int countOfAlivePlayersAndBots = alivePlayersAndBots.count;
            int countOfKilledEntities = killedEntities.Count;
            
            foreach (var player in alivePlayers)
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
                        TargetPlayerId = player.player.id,
                        KillerId = killerInfo.playerId,
                        KillerType = killerInfo.type,
                        VictimType = killedEntity.viewType.id,
                        VictimId = killedEntity.player.id
                    };
                
                    UdpSendUtils.SendKill(matchId, killData);

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
                        
                        PlayerDeathNotifier.KilledPlayers.Enqueue(playerDeathData);
                        UdpSendUtils.SendBattleFinishMessage(matchId, killedEntity.player.id);


                        bool success = match.TryRemoveIpEndPoint(playerId);
                        if (!success)
                        { 
                            throw new Exception("Получив это сообщение, я буду пребывать в крайне скудном расположении духа.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Если за один кадр умерло больше двух игроков, то им выдадутся разные места в бою.
        /// </summary>
        private int GetPlaceInBattle(int countOfAlivePlayersAndBots, int countOfKilledEntities, int killedEntityIndex)
        {
            return countOfAlivePlayersAndBots + 1 + (countOfKilledEntities - 1 - killedEntityIndex);
        }
    }
}
