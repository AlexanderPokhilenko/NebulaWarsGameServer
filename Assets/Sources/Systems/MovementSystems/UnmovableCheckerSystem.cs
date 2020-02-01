using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class UnmovableCheckerSystem : ReactiveSystem<GameEntity>
{

    public UnmovableCheckerSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Unmovable.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasVelocity && entity.isUnmovable;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            e.RemoveVelocity();
        }
    }
}