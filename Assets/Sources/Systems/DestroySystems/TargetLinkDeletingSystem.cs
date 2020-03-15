using Entitas;
using System.Collections.Generic;

public sealed class TargetLinkDeletingSystem : IExecuteSystem
{
    private readonly List<GameEntity> buffer;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> withTargetGroup;

    public TargetLinkDeletingSystem(Contexts contexts)
    {
        buffer = new List<GameEntity>();
        gameContext = contexts.game;
        withTargetGroup = gameContext.GetGroup(GameMatcher.Target);
    }

    public void Execute()
    {
        foreach (var e in withTargetGroup.GetEntities(buffer))
        {
            var target = gameContext.GetEntityWithId(e.target.id);
            if (target.isDestroyed)
            {
                e.RemoveTarget();
            }
        }
    }
}
