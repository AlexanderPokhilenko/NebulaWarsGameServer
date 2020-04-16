using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using UnityEngine;

public sealed class LifetimeSubtractionSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> lifetimeGroup;

    public LifetimeSubtractionSystem(Contexts contexts)
    {
        lifetimeGroup = contexts.game.GetGroup(GameMatcher.Lifetime);
    }

    public void Execute()
    {
        foreach (var e in lifetimeGroup)
        {
            e.ReplaceLifetime(e.lifetime.value - Chronometer.GetMagicDich());
        }
    }
}
