using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class MovingAnimationSystem : ReactiveSystem<GameEntity>
{

    public MovingAnimationSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Velocity.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAnimator;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            e.animator.value.SetBool("Moving", e.hasVelocity);
        }
    }
}