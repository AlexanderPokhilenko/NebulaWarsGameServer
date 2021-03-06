﻿using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using Server.GameEngine.Chronometers;
using UnityEngine;

public sealed class RotatingSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> rotatableGroup;

    public RotatingSystem(Contexts contexts)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.AngularVelocity, GameMatcher.Direction);
        rotatableGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in rotatableGroup)
        {
            var newAngle = e.direction.angle + e.angularVelocity.value * Chronometer.DeltaTime;
            while (newAngle >= 360) newAngle -= 360;
            while (newAngle <= 0) newAngle += 360;
            // Возможно, стоит делать угол от -180 до 180, а не от 0 до 360
            e.ReplaceDirection(newAngle);
        }
    }
}
