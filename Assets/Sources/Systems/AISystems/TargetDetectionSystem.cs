using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public sealed class TargetDetectionSystem : IExecuteSystem
{
    private readonly List<GameEntity> buffer;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> targetingGroup;
    private readonly IGroup<GameEntity> targetGroup;

    public TargetDetectionSystem(Contexts contexts)
    {
        buffer = new List<GameEntity>();
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.TargetingParameters).NoneOf(GameMatcher.Target);
        targetingGroup = gameContext.GetGroup(matcher);
        var targetMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.HealthPoints, GameMatcher.Collidable, GameMatcher.CircleCollider).NoneOf(GameMatcher.PassingThrough);
        targetGroup = gameContext.GetGroup(targetMatcher);
    }

    public void Execute()
    {
        foreach (var e in targetingGroup.GetEntities(buffer))
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
                        currentVal = sqrDirection - target.circleCollider.radius * target.circleCollider.radius;
                    }

                    // Установка приоритетности цели:
                    // стреляем по игрокам с большей вероятностью
                    if (target.hasPlayer) currentVal *= 0.5f;
                    // обращаем внимание на целящиеся объекты
                    var targetsTargetingChildren = target.GetAllChildrenGameEntities(gameContext, c => c.hasTargetingParameters);
                    foreach (var targetingChild in targetsTargetingChildren)
                    {
                        var childTargetingRadius = targetingChild.targetingParameters.radius;
                        if (sqrDirection <= childTargetingRadius * childTargetingRadius) currentVal *= 0.5f;
                    }
                    // нужно проверить, стреляют ли в нас
                    var targetsChildrenWithTarget = target.GetAllChildrenGameEntities(gameContext, c => c.hasTarget);
                    foreach (var childWithTarget in targetsChildrenWithTarget)
                    {
                        var childTarget = gameContext.GetEntityWithId(childWithTarget.target.id);
                        if (childTarget.GetGrandOwnerId(gameContext) == currentGrandOwnerId) currentVal *= 0.05f;
                    }
                    // летят ли в нас выстрелы
                    var targetsDirectionTargetingChildrenCount = target.GetAllChildrenGameEntities(gameContext, c => c.hasDirectionTargeting).Count();
                    if(targetsDirectionTargetingChildrenCount > 0)
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
                        if (antiOverlap < 1f) currentVal *= antiOverlap / targetsDirectionTargetingChildrenCount;
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
