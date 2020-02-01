using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class AddingCannonCooldownSystem : ReactiveSystem<GameEntity>
{

    public AddingCannonCooldownSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TryingToShoot.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.hasCannonCooldown && entity.hasCannon;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            e.AddCannonCooldown(e.cannon.cooldown);
        }
    }
}