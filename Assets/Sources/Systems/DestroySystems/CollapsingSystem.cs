using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class CollapsingSystem : ReactiveSystem<GameEntity>
{
    public CollapsingSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.Collapses, GameMatcher.Collided);
        return context.CreateCollector(matcher);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollapses && entity.isCollided && !entity.isDestroyed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isDestroyed = true;
        }
    }
}