using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public sealed class TargetDetectionSystem : IExecuteSystem
{
    private readonly List<GameEntity> buffer;
    private readonly List<Target> targets;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> targetingGroup;
    private readonly IGroup<GameEntity> targetGroup;
    private const int predictedCapacity = 320;

    public TargetDetectionSystem(Contexts contexts)
    {
        buffer = new List<GameEntity>();
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.TargetingParameters).NoneOf(GameMatcher.Target);
        targetingGroup = gameContext.GetGroup(matcher);
        var targetMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.HealthPoints, GameMatcher.Collidable, GameMatcher.CircleCollider).NoneOf(GameMatcher.PassingThrough);
        targetGroup = gameContext.GetGroup(targetMatcher);
        targets = new List<Target>(predictedCapacity);
    }

    private class Target
    {
        public readonly int Id;
        public readonly float Radius;
        public readonly float SqrRadius;
        public readonly int GrandOwnerId;
        public readonly Vector2 GlobalPosition;
        public readonly float NegativeAngleSin;
        public readonly float NegativeAngleCos;
        public readonly bool IsPlayer;
        public readonly float[] SqrChildrenTargetingRadiuses;
        public readonly int[] ChildrenGrandTargetIds;
        public readonly int DirectionTargetingChildrenCount;

        public Target(GameEntity entity, GameContext gameContext)
        {
            Id = entity.id.value;
            Radius = entity.circleCollider.radius;
            SqrRadius = Radius * Radius;
            float globalAngle;
            if (entity.hasGlobalTransform)
            {
                GlobalPosition = entity.globalTransform.position;
                globalAngle = entity.globalTransform.angle;
            }
            else
            {
                entity.ToGlobal(gameContext, out GlobalPosition, out globalAngle, out _, out _, out _);
            }
            CoordinatesExtensions.GetSinCosFromDegrees(-globalAngle, out NegativeAngleSin, out NegativeAngleCos);
            GrandOwnerId = entity.hasGrandOwner ? entity.grandOwner.id : Id;
            IsPlayer = entity.hasPlayer;
            SqrChildrenTargetingRadiuses = entity.GetAllChildrenGameEntities(gameContext, c => c.hasTargetingParameters)
                .Select(c => c.targetingParameters.radius).Select(r => r * r).ToArray();
            ChildrenGrandTargetIds = entity.GetAllChildrenGameEntities(gameContext, c => c.hasTarget)
                .Select(c => gameContext.GetEntityWithId(c.target.id))
                .Select(t => t.hasGrandOwner ? t.grandOwner.id : t.GetGrandOwnerId(gameContext)).ToArray();
            DirectionTargetingChildrenCount = entity.GetAllChildrenGameEntities(gameContext, c => c.hasDirectionTargeting).Count();
        }
    }

    public void Execute()
    {
        foreach (var targetEntity in targetGroup)
        {
            targets.Add(new Target(targetEntity, gameContext));
        }
        foreach (var e in targetingGroup.GetEntities(buffer))
        {
            var currentPosition = e.GetGlobalPositionVector2(gameContext);
            var currentDirection = CoordinatesExtensions.GetRotatedUnitVector2(e.GetGlobalAngle(gameContext));
            var onlyPlayerTargeting = e.targetingParameters.onlyPlayerTargeting;
            var targetingRadius = e.targetingParameters.radius;
            var sqrTargetingRadius = targetingRadius * targetingRadius;
            var grandParent = e.GetGrandParent(gameContext);
            var currentGrandOwnerId = e.hasGrandOwner ? e.grandOwner.id : e.GetGrandOwnerId(gameContext);
            var minVal = float.PositiveInfinity;
            var targetId = 0;
            var targetFound = false;
            foreach (var target in targets)
            {
                if (onlyPlayerTargeting && !target.IsPlayer) continue;
                if (e.id.value == target.Id || /*e.IsParentOf(target, gameContext) || target.IsParentOf(e, gameContext) ||*/ currentGrandOwnerId == target.GrandOwnerId) continue;
                var targetPosition = target.GlobalPosition;
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
                        currentVal = sqrDirection - target.SqrRadius;
                    }

                    // Установка приоритетности цели:
                    // стреляем по игрокам с большей вероятностью
                    if (target.IsPlayer) currentVal *= 0.5f;
                    // обращаем внимание на целящиеся объекты
                    foreach (var sqrChildTargetingRadius in target.SqrChildrenTargetingRadiuses)
                    {
                        if (sqrDirection <= sqrChildTargetingRadius) currentVal *= 0.5f;
                    }
                    // нужно проверить, стреляют ли в нас
                    foreach (var childGrandTargetId in target.ChildrenGrandTargetIds)
                    {
                        if (childGrandTargetId == currentGrandOwnerId) currentVal *= 0.05f;
                    }
                    // летят ли в нас выстрелы
                    if(target.DirectionTargetingChildrenCount > 0)
                    {
                        var grandParentDirection = targetPosition - grandParent.GetGlobalPositionVector2(gameContext);
                        var localTargetPosition = grandParentDirection.GetRotated(target.NegativeAngleSin, target.NegativeAngleCos);
                        var absProjectionY = Mathf.Abs(localTargetPosition.y);
                        var overlapLine = target.Radius;
                        if (grandParent.hasCircleCollider)
                        {
                            overlapLine += grandParent.circleCollider.radius;
                        }
                        var antiOverlap = absProjectionY / overlapLine;
                        if (antiOverlap < 1f) currentVal *= antiOverlap / target.DirectionTargetingChildrenCount;
                    }

                    if (currentVal < minVal)
                    {
                        minVal = currentVal;
                        targetId = target.Id;
                    }
                }
            }
            if(targetFound) e.AddTarget(targetId);
        }

        targets.Clear();
    }
}
