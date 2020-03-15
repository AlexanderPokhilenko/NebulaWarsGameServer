using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class RemoveTargetForSingleTargetingSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> singleTargetingGroup;

    public RemoveTargetForSingleTargetingSystem(Contexts contexts)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.SingleTargeting, GameMatcher.TryingToShoot);
        singleTargetingGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in singleTargetingGroup)
        {
            if(e.hasTarget) e.RemoveTarget();
            if (e.hasDirectionTargeting) e.RemoveDirectionTargeting();
            if (e.hasAngularVelocity) e.RemoveAngularVelocity();
            e.isDirectionTargetingShooting = false;
        }
    }
}
