using Entitas;
using System.Collections.Generic;
using Server.Udp.Sending;

public class NetworkKillsSenderSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> players;
    private readonly Dictionary<int, (int playerId, ViewTypeId type)> killersInfo;

    public NetworkKillsSenderSystem(Contexts contexts, Dictionary<int, (int playerId, ViewTypeId type)> killersInfos) : base(contexts.game)
    {
        killersInfo = killersInfos;
        gameContext = contexts.game;
        players = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.KilledBy.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPlayer && entity.hasViewType && entity.hasKilledBy;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var player in players)
        {
            foreach (GameEntity e in entities)
            {
                if (!killersInfo.TryGetValue(e.killedBy.id, out var killerInfo))
                {
                    killerInfo = (0, 0);
                }
                UdpSendUtils.SendKills(player.player.id, killerInfo.playerId, killerInfo.type, e.player.id, e.viewType.id);
            }
        }
    }
}