using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using UnityEngine;

public sealed class CircleTargetScalingCheckerSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;

    public CircleTargetScalingCheckerSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CircleCollider);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCircleScaling && entity.hasTargetScaling;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var scalingSpeed = e.circleScaling.speed;
            var currentRadius = e.circleCollider.radius;
            var targetRadius = e.targetScaling.radius;
            if ((scalingSpeed > 0 && currentRadius >= targetRadius)
                || (scalingSpeed < 0 && currentRadius <= targetRadius))
            {
                var deltaRadius = targetRadius - currentRadius;
                if (Mathf.Abs(scalingSpeed) * Chronometer.DeltaTime >= Mathf.Abs(deltaRadius))
                {
                    e.circleCollider.radius = e.targetScaling.radius;
                    e.RemoveTargetScaling();
                    e.RemoveCircleScaling();
                }
                else
                {
                    e.ReplaceCircleScaling(-scalingSpeed);
                }
            }
        }
    }
}