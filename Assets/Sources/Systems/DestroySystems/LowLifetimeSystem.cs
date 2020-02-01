using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class LowLifetimeSystem : ReactiveSystem<GameEntity>
{
    public LowLifetimeSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Lifetime);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLifetime && !entity.isDestroyed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if(e.lifetime.value <= 0) e.isDestroyed = true;
        }
    }
}