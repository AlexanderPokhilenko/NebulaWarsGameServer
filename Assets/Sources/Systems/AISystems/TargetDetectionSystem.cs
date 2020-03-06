using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class TargetDetectionSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> targetingGroup;
    private IGroup<GameEntity> targetGroup;

    public TargetDetectionSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.TargetingParameters).NoneOf(GameMatcher.Target);
        targetingGroup = gameContext.GetGroup(matcher);
        var targetMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.HealthPoints, GameMatcher.Collidable, GameMatcher.CircleCollider).NoneOf(GameMatcher.PassingThrough);
        targetGroup = gameContext.GetGroup(targetMatcher);
    }

    public void Execute()
    {
        //TODO: посмотреть, можно ли как-то добавлять без использования массива
        foreach (var e in targetingGroup.GetEntities())
        {
            var currentPosition = e.GetGlobalPositionVector2(gameContext);
            var currentDirection = CoordinatesExtensions.GetRotatedUnitVector2(e.GetGlobalAngle(gameContext));
            var onlyPlayerTargeting = e.targetingParameters.onlyPlayerTargeting;
            var targetingRadius = e.targetingParameters.radius;
            var sqrTargetingRadius = targetingRadius * targetingRadius;
            var minVal = float.PositiveInfinity;
            var targetId = 0;
            var targetFound = false;
            foreach (var target in targetGroup)
            {
                if(e.IsParentOf(target, gameContext) || target.IsParentOf(e, gameContext)) continue;
                if(onlyPlayerTargeting && !target.hasPlayer) continue;
                var targetPosition = target.GetGlobalPositionVector2(gameContext);
                var direction = targetPosition - currentPosition;
                var sqrDirection = direction.sqrMagnitude;
                if (sqrDirection <= sqrTargetingRadius)
                {
                    targetFound = true;
                    if (e.targetingParameters.angularTargeting)
                    {
                        var targetAngle = Vector2.Angle(currentDirection, direction);
                        if (targetAngle < minVal)
                        {
                            minVal = targetAngle;
                            targetId = target.id.value;
                        }
                    }
                    else
                    {
                        if (sqrDirection < minVal)
                        {
                            minVal = sqrDirection;
                            targetId = target.id.value;
                        }
                    }
                }
            }
            if(targetFound) e.AddTarget(targetId);
        }
    }
}
