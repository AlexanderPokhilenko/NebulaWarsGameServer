using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ParentFixedRemoverSystem : ReactiveSystem<GameEntity>
{

    public ParentFixedRemoverSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Parent.Removed());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isUnmovable && entity.isParentFixed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            e.isUnmovable = false;
            e.isParentFixed = false;
        }
    }
}