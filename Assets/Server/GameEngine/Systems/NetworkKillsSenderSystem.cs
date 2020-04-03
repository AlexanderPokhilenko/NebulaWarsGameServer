using Entitas;
using System.Collections.Generic;
using log4net;
using Server.GameEngine;
using Server.Http;
using Server.Udp.Sending;

/// <summary>
/// Отвечает за отправку сообщения об убийствах.
/// </summary>
public class NetworkKillsSenderSystem : ReactiveSystem<GameEntity>
{
    readonly IGroup<GameEntity> alivePlayersAndBots;
    private readonly IGroup<GameEntity> alivePlayers;
    private readonly Dictionary<int, (int playerId, ViewTypeId type)> killersInfo;
    private static readonly ILog Log = LogManager.GetLogger(typeof(NetworkKillsSenderSystem));
    
    public NetworkKillsSenderSystem(Contexts contexts, Dictionary<int, (int playerId, ViewTypeId type)> killersInfos)
        : base(contexts.game)
    {
        killersInfo = killersInfos;
        var gameContext = contexts.game;
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

        if (killedEntities.Count > 1)
        {
            Log.Warn($"killedEntities.Count = {killedEntities.Count}");
        }
        
        foreach (var player in alivePlayers)
        {
            for (var killedEntityIndex = 0; killedEntityIndex < killedEntities.Count; killedEntityIndex++)
            {
                GameEntity killedEntity = killedEntities[killedEntityIndex];
                if (!killersInfo.TryGetValue(killedEntity.killedBy.id, out var killerInfo))
                {
                    killerInfo = (0, 0);
                }

                UdpSendUtils.SendKills(player.player.id, killerInfo.playerId,
                    killerInfo.type, killedEntity.player.id, killedEntity.viewType.id);

                if (!killedEntity.isBot)
                {
                    int playerTmpId = killedEntity.player.id;
                    int matchId = BattlesStorage.Instance.GetMatchId(playerTmpId);
                    int placeInBattle = GetPlaceInBattle(countOfAlivePlayersAndBots, countOfKilledEntities, killedEntityIndex);
                    PlayerDeathData playerDeathData = new PlayerDeathData
                    {
                        PlayerId = playerTmpId,
                        PlaceInBattle = placeInBattle,
                        MatchId = matchId 
                    };
                    PlayerDeathNotifier.KilledPlayerIds.Enqueue(playerDeathData);
                    UdpSendUtils.SendBattleFinishMessage(killedEntity.player.id);    
                }
            }
        }
    }

    private int GetPlaceInBattle(int countOfAlivePlayersAndBots, int countOfKilledEntities, int killedEntityIndex)
    {
        return countOfAlivePlayersAndBots + 1 + (countOfKilledEntities - 1 - killedEntityIndex);
    }
}
