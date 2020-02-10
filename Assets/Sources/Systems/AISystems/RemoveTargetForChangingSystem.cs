using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class RemoveTargetForChangingSystem : ICleanupSystem
{
    private IGroup<GameEntity> changingGroup;

    public RemoveTargetForChangingSystem(Contexts contexts)
    {
        changingGroup = contexts.game.GetGroup(GameMatcher.TargetChanging);
    }

    public void Cleanup()
    {
        foreach (var e in changingGroup)
        {
            if(e.hasTarget) e.RemoveTarget();
            if (e.hasDirectionTargeting) e.RemoveDirectionTargeting();
            if (e.hasAngularVelocity) e.RemoveAngularVelocity();
        }
    }
}
