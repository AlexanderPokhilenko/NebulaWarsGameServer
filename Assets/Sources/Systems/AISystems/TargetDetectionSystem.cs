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
            var grandParent = e.GetGrandParent(gameContext);
            var currentGrandOwnerId = e.GetGrandOwnerId(gameContext);
            var minVal = float.PositiveInfinity;
            var targetId = 0;
            var targetFound = false;
            foreach (var target in targetGroup)
            {
                if(e.id.value == target.id.value || /*e.IsParentOf(target, gameContext) || target.IsParentOf(e, gameContext) ||*/ e.GetGrandOwnerId(gameContext) == target.GetGrandOwnerId(gameContext)) continue;
                if(onlyPlayerTargeting && !target.hasPlayer) continue;
                var targetPosition = target.GetGlobalPositionVector2(gameContext);
                var direction = targetPosition - currentPosition;
                var sqrDirection = direction.sqrMagnitude;
                if (sqrDirection <= sqrTargetingRadius)
                {
                    targetFound = true;
                    float currentVal;
                    if (e.targetingParameters.angularTargeting)
                    {
                        currentVal = Vector2.Angle(currentDirection, direction);
                    }
                    else
                    {
                        currentVal = sqrDirection;
                    }

                    // Установка приоритетности цели:
                    // стреляем по игрокам с большей вероятностью
                    if (target.hasPlayer) currentVal *= 0.5f;
                    // обращаем внимание на целящиеся объекты
                    if (target.hasTargetingParameters)
                    {
                        var targetTargetingRadius = target.targetingParameters.radius;
                        if (sqrDirection <= targetTargetingRadius * targetTargetingRadius) currentVal *= 0.5f;
                    }
                    // нужно проверить, стреляют ли в нас
                    if (target.hasTarget)
                    {
                        var targetsTarget = gameContext.GetEntityWithId(target.target.id);
                        if (targetsTarget.GetGrandOwnerId(gameContext) == currentGrandOwnerId) currentVal *= 0.05f;
                    }
                    // летят ли в нас выстрелы
                    if (target.hasDirectionTargeting)
                    {
                        var grandParentDirection = targetPosition - grandParent.GetGlobalPositionVector2(gameContext);
                        var localTargetPosition = target.GetLocalRotatedVector(gameContext, grandParentDirection);
                        var absProjectionY = Mathf.Abs(localTargetPosition.y);
                        var overlapLine = target.circleCollider.radius;
                        if (grandParent.hasCircleCollider)
                        {
                            overlapLine += grandParent.circleCollider.radius;
                        }
                        var antiOverlap = absProjectionY / overlapLine;
                        if (antiOverlap < 1f) currentVal *= antiOverlap;
                    }

                    if (currentVal < minVal)
                    {
                        minVal = currentVal;
                        targetId = target.id.value;
                    }
                }
            }
            if(targetFound) e.AddTarget(targetId);
        }
    }
}
