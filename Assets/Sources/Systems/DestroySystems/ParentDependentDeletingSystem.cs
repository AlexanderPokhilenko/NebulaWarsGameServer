using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class ParentDependentDeletingSystem : ReactiveSystem<GameEntity>
{
    public readonly GameContext gameContext;

    public ParentDependentDeletingSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroyed.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDestroyed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            DestroyWithDependentChildren(e);
        }

        void DestroyWithDependentChildren(GameEntity parent)
        {
            parent.isDestroyed = true;

            var dependentChildren = gameContext.GetEntitiesWithParent(parent.id.value).Where(c => c.isParentDependent);

            foreach (var dependentChild in dependentChildren)
            {
                DestroyWithDependentChildren(dependentChild);
            }
        }
    }
}