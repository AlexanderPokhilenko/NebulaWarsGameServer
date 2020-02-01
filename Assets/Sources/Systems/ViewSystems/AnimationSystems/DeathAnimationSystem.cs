using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class DeathAnimationSystem : ReactiveSystem<GameEntity>
{

    public DeathAnimationSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroyed.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAnimator;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            e.animator.value.SetTrigger("Death");
        }
    }
}