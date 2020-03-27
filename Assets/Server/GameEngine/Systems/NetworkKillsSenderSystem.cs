using System;
using Entitas;
using System.Collections.Generic;
using log4net;
using Server.Http;
using Server.Udp.Sending;
using UnityEngine;

public class NetworkKillsSenderSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;
    IGroup<GameEntity> alivePlayersAndBots;
    private readonly IGroup<GameEntity> alivePlayers;
    private readonly Dictionary<int, (int playerId, ViewTypeId type)> killersInfo;
    private static readonly ILog Log = LogManager.GetLogger(typeof(NetworkKillsSenderSystem));
    
    public NetworkKillsSenderSystem(Contexts contexts, Dictionary<int, (int playerId, ViewTypeId type)> killersInfos) : base(contexts.game)
    {
        killersInfo = killersInfos;
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

        if (killedEntities.Count > 1)
        {
            Log.Warn($"killedEntities.Count = {killedEntities.Count}");
        }
        
        foreach (var player in alivePlayers)
        {
            for (var killedEntityIndex = 0; killedEntityIndex < killedEntities.Count; killedEntityIndex++)
            {
                GameEntity e = killedEntities[killedEntityIndex];
                if (!killersInfo.TryGetValue(e.killedBy.id, out var killerInfo))
                {
                    killerInfo = (0, 0);
                }

                UdpSendUtils.SendKills(player.player.id, killerInfo.playerId,
                    killerInfo.type, e.player.id, e.viewType.id);

                int placeInBattle = GetPlaceInBattle(countOfAlivePlayersAndBots, countOfKilledEntities, killedEntityIndex);
                PlayerDeathData playerDeathData = new PlayerDeathData
                {
                    PlayerId = e.player.id,
                    PlaceInBattle = placeInBattle
                };

                PlayerDeathNotifier.KilledPlayerIds.Enqueue(playerDeathData);
            }
        }
    }

    private int GetPlaceInBattle(int countOfAlivePlayersAndBots, int countOfKilledEntities, int killedEntityIndex)
    {
        return countOfAlivePlayersAndBots + 1 + (countOfKilledEntities - 1 - killedEntityIndex);
    }
}
