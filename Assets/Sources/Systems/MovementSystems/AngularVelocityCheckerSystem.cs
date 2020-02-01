using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class AngularVelocityCheckerSystem : ReactiveSystem<GameEntity>
{
    private const float stopAngularVelocity = 0.0001f;

    public AngularVelocityCheckerSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.AngularVelocity, GameMatcher.MaxAngularVelocity);
        return context.CreateCollector(matcher);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAngularVelocity && entity.hasMaxAngularVelocity;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            var absVelocity = Mathf.Abs(e.angularVelocity.value);
            if (absVelocity <= stopAngularVelocity)
            {
                e.RemoveAngularVelocity();
            }
            else if (absVelocity > e.maxAngularVelocity.value)
            {
                int sign = Math.Sign(e.angularVelocity.value);
                e.ReplaceAngularVelocity(sign * e.maxAngularVelocity.value);
            }
        }
    }
}