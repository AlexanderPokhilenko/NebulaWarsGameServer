using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using UnityEngine;

public sealed class DecelerateSystem : IExecuteSystem
{
    private const float deceleratingConstant = 0.5f;
    private readonly IGroup<GameEntity> movableGroup;

    public DecelerateSystem(Contexts contexts)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.Velocity).NoneOf(GameMatcher.Unmovable, GameMatcher.NotDecelerating);
        movableGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in movableGroup)
        {
            //TODO: заменить на нормальную формулу
            var newVelocity = e.velocity.value / (1 + deceleratingConstant * Chronometer.DeltaTime);
            e.ReplaceVelocity(newVelocity);
        }
    }
}
