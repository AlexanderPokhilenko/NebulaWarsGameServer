using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class AddCollidersToCircleSystem : ReactiveSystem<GameEntity>
{
    public AddCollidersToCircleSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CircleCollider.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollidable && !entity.hasRectangleCollider && !entity.hasPathCollider;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isRound = true;
        }
    }
}