using System.Collections;
using System.Collections.Generic;
using AmoebaBattleServer01.Experimental.GameEngine;
using Entitas;
using UnityEngine;

public sealed class RotatingSystem : IExecuteSystem
{
    private IGroup<GameEntity> rotatableGroup;

    public RotatingSystem(Contexts contexts)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.AngularVelocity, GameMatcher.Direction);
        rotatableGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in rotatableGroup)
        {
            var newAngle = e.direction.angle + e.angularVelocity.value * Clock.deltaTime;
            while (newAngle >= 360) newAngle -= 360;
            while (newAngle <= 0) newAngle += 360;
            // Возможно, стоит делать угол от -180 до 180, а не от 0 до 360
            e.ReplaceDirection(newAngle);
        }
    }
}
