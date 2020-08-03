using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using Server.GameEngine.Chronometers;
using UnityEngine;

public sealed class CircleScalingSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> scalingGroup;

    public CircleScalingSystem(Contexts contexts)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.CircleScaling, GameMatcher.CircleCollider);
        scalingGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in scalingGroup)
        {
            var delta = e.circleScaling.speed * Chronometer.DeltaTime;
            e.ReplaceCircleCollider(e.circleCollider.radius + delta);
        }
    }
}
