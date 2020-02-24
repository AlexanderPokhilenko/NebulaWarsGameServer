using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class RemoveDistantTargetSystem : ICleanupSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> targetingGroup;

    public RemoveDistantTargetSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.TargetingParameters, GameMatcher.Target);
        targetingGroup = gameContext.GetGroup(matcher);
    }

    public void Cleanup()
    {
        //TODO: посмотреть, можно ли как-то удалять без использования массива
        foreach (var e in targetingGroup.GetEntities())
        {
            var target = gameContext.GetEntityWithId(e.target.id);
            var targetPosition = target.GetGlobalPositionVector2(gameContext);
            var currentPosition = e.GetGlobalPositionVector2(gameContext);
            var targetingRadius = e.targetingParameters.radius;
            var direction = targetPosition - currentPosition;
            if (direction.sqrMagnitude > targetingRadius * targetingRadius)
            {
                e.RemoveTarget();
                e.RemoveDirectionTargeting();
                e.RemoveAngularVelocity();
            }
        }
    }
}
