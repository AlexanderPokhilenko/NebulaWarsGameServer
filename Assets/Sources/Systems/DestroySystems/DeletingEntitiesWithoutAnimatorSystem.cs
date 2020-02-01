using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class DeletingEntitiesWithoutAnimatorSystem : ReactiveSystem<GameEntity>
{
    public DeletingEntitiesWithoutAnimatorSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroyed.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.hasAnimator;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isNeededToDelete = true;
        }
    }
}