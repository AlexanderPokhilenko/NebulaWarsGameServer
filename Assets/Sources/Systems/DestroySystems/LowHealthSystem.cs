using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class LowHealthSystem : ReactiveSystem<GameEntity>
{
    public LowHealthSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.HealthPoints);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasHealthPoints && !entity.isInvulnerable && !entity.isDestroyed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if(e.healthPoints.value <= 0) e.isDestroyed = true;
        }
    }
}