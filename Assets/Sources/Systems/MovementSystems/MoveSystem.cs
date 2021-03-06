﻿using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using Server.GameEngine.Chronometers;
using UnityEngine;

public sealed class MoveSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> movableGroup;

    public MoveSystem(Contexts contexts)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.Velocity, GameMatcher.Position).NoneOf(GameMatcher.Unmovable);
        movableGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in movableGroup)
        {
            e.ReplacePosition(e.position.value + e.velocity.value * Chronometer.DeltaTime);
        }
    }
}
