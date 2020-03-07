using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Entitas;
using UnityEngine;

public sealed class BotsMovingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> botsGroup;
    private const float zoneWarningDistance = 1.5f;

    public BotsMovingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.CircleCollider, GameMatcher.Bot).NoneOf(GameMatcher.TargetMovingPoint);
        botsGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        //TODO: посмотреть, можно ли как-то добавлять без использования массива
        foreach (var e in botsGroup.GetEntities())
        {
            var zone = gameContext.zone.GetZone(gameContext);
            var currentPosition = e.GetGlobalPositionVector2(gameContext);
            var zonePosition = zone.GetGlobalPositionVector2(gameContext);
            var zoneRadius = zone.circleCollider.radius;

            float maxRadius = (e.circleCollider.radius + Random.Range(0, e.maxVelocity.value));
            Vector2 targetPosition = currentPosition + maxRadius * CoordinatesExtensions.GetRandomUnitVector2();

            //if (e.hasTargetingParameters)
            //{
            //    maxRadius = e.targetingParameters.radius;
            //    targetPosition = Random.Range(e.circleCollider.radius, maxRadius) * CoordinatesExtensions.GetRandomUnitVector2();
            //    targetPosition += currentPosition;
            //}
            //else
            //{
            //    maxRadius = zoneRadius;
            //    targetPosition = Random.Range(e.circleCollider.radius, maxRadius) * CoordinatesExtensions.GetRandomUnitVector2();
            //    targetPosition += zonePosition;
            //}

            var safeRadius = zoneRadius - zoneWarningDistance;

            if (targetPosition.sqrMagnitude >= safeRadius * safeRadius)
            {
                var vectorToCenter = zonePosition - currentPosition;
                var savingVector = Random.Range(zoneWarningDistance, maxRadius) * vectorToCenter.normalized;
                targetPosition += savingVector;
            }

            e.AddTargetMovingPoint(targetPosition);
        }
    }
}
