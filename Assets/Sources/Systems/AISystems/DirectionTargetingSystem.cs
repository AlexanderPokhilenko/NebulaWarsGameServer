using System.Collections;
using System.Collections.Generic;
using AmoebaBattleServer01.Experimental.GameEngine;
using Entitas;
using UnityEngine;

public sealed class DirectionTargetingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> targetingGroup;

    public DirectionTargetingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.Target);
        targetingGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in targetingGroup)
        {
            var currentPosition = e.GetGlobalPositionVector2(gameContext);
            var currentDirection = Vector2.right.GetRotated(e.GetGlobalAngle(gameContext));

            var target = gameContext.GetEntityWithId(e.target.id);
            var targetPosition = target.GetGlobalPositionVector2(gameContext);

            var direction = targetPosition - currentPosition;

            var targetAngle = Vector2.SignedAngle(currentDirection, direction);

            var angularVelocity = targetAngle / Clock.deltaTime;
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
