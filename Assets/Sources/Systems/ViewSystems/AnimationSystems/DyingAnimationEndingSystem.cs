using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class DyingAnimationEndingSystem : IExecuteSystem
{
    private IGroup<GameEntity> dyingGroup;

    public DyingAnimationEndingSystem(Contexts contexts)
    {
        var matcher = GameMatcher.AllOf(GameMatcher.Destroyed, GameMatcher.Animator);
        dyingGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in dyingGroup)
        {
            var animatorState = e.animator.value.GetCurrentAnimatorStateInfo(0);
            if (animatorState.IsName("Death") && animatorState.normalizedTime >= 1f) e.isNeededToDelete = true;
        }
    }
}