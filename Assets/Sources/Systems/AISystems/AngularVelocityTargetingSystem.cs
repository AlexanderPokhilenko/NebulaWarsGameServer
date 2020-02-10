using System.Collections;
using System.Collections.Generic;
using AmoebaBattleServer01.Experimental.GameEngine;
using Entitas;
using UnityEngine;

public sealed class AngularVelocityTargetingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> targetingGroup;

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
            var targetAngle = e.directionTargeting.angle;

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
