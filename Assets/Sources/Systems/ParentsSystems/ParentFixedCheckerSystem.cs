using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ParentFixedCheckerSystem : ReactiveSystem<GameEntity>
{

    public ParentFixedCheckerSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ParentFixed.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.isUnmovable && entity.hasParent;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            e.isUnmovable = true;
        }
    }
}