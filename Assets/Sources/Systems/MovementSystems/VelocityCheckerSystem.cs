using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class VelocityCheckerSystem : ReactiveSystem<GameEntity>
{
    private const float stopSqrMagnitude = 0.0001f;

    public VelocityCheckerSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.Velocity, GameMatcher.MaxVelocity);
        return context.CreateCollector(matcher);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasVelocity && entity.hasMaxVelocity;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            if (e.velocity.value.sqrMagnitude <= stopSqrMagnitude)
            {
                e.RemoveVelocity();
            }
            else if (e.velocity.value.sqrMagnitude > e.maxVelocity.value * e.maxVelocity.value)
            {
                var direction = e.velocity.value.normalized;
                e.ReplaceVelocity(direction * e.maxVelocity.value);
            }
        }
    }
}