using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class RemovingNegativeCannonCooldownSystem : ReactiveSystem<GameEntity>
{

    public RemovingNegativeCannonCooldownSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CannonCooldown);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCannonCooldown;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            if(e.cannonCooldown.value <= 0) e.RemoveCannonCooldown();
        }
    }
}