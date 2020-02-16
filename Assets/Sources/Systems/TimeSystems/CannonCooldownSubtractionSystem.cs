using System.Collections;
using System.Collections.Generic;
using Entitas;
using OldServer.Experimental.GameEngine;
using UnityEngine;

public sealed class CannonCooldownSubtractionSystem : IExecuteSystem
{
    private IGroup<GameEntity> cooldownGroup;

    public CannonCooldownSubtractionSystem(Contexts contexts)
    {
        cooldownGroup = contexts.game.GetGroup(GameMatcher.CannonCooldown);
    }

    public void Execute()
    {
        foreach (var e in cooldownGroup)
        {
            e.ReplaceCannonCooldown(e.cannonCooldown.value - Clock.deltaTime);
        }
    }
}
