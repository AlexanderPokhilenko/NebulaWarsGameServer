using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using UnityEngine;

public sealed class TargetPointMovingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> movingGroup;
    private const float positionSqrDelta = 0.01f;

    public TargetPointMovingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.MaxVelocity, GameMatcher.TargetMovingPoint).NoneOf(GameMatcher.Unmovable);
        movingGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        //TODO: посмотреть, можно ли как-то удалять без использования массива
        foreach (var e in movingGroup.GetEntities())
        {
            var delta = e.targetMovingPoint.position - e.GetGlobalPositionVector2(gameContext);
            var deltaSqrMagnitude = delta.sqrMagnitude;
            if (deltaSqrMagnitude <= positionSqrDelta)
            {
                e.RemoveTargetMovingPoint();
                continue;
            }

            var newVelocity = delta / Clock.deltaTime;
            if (e.hasVelocity)
            {
                e.ReplaceVelocity(newVelocity);
            }
            else
            {
                e.AddVelocity(newVelocity);
            }

            if (!e.isDirectionTargetingShooting)
            {
                var directionAngle = Mathf.Atan2(newVelocity.y, newVelocity.x) * Mathf.Rad2Deg;
                if (directionAngle < 0) directionAngle += 360f;
                if (e.hasDirectionTargeting)
                {
                    e.ReplaceDirectionTargeting(directionAngle);
                }
                else
                {
                    e.AddDirectionTargeting(directionAngle);
                }
            }
        }
    }
}
