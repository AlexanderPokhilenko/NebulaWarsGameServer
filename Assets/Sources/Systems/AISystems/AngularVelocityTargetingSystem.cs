using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using UnityEngine;

public sealed class AngularVelocityTargetingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> targetingGroup;

    public AngularVelocityTargetingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.DirectionTargeting);
        targetingGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in targetingGroup)
        {
            var currentAngle = e.GetGlobalAngle(gameContext);

            var targetAngle = e.directionTargeting.angle;

            var rotatingDelta = targetAngle - currentAngle;

            if (rotatingDelta > 180f)
            {
                rotatingDelta -= 360f;
            }
            else if (rotatingDelta < -180f)
            {
                rotatingDelta += 360f;
            }

            var angularVelocity = rotatingDelta / Chronometer.DeltaTime;
            //TODO: возможно, стоит учитывать глобальную угловую скорость
            if (e.hasAngularVelocity)
            {
                e.ReplaceAngularVelocity(angularVelocity);
            }
            else
            {
                e.AddAngularVelocity(angularVelocity);
            }
        }
    }
}