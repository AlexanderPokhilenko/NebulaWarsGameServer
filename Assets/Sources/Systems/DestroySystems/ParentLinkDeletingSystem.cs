using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public sealed class ParentLinkDeletingSystem : IExecuteSystem
{
    private readonly List<GameEntity> buffer;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> withParentGroup;

    public ParentLinkDeletingSystem(Contexts contexts)
    {
        buffer = new List<GameEntity>();
        gameContext = contexts.game;
        withParentGroup = gameContext.GetGroup(GameMatcher.Parent);
    }

    public void Execute()
    {
        foreach (var e in withParentGroup.GetEntities(buffer))
        {
            var parent = gameContext.GetEntityWithId(e.parent.id);
            if (parent.isDestroyed)
            {
                e.ToGlobal(gameContext, out var globalPosition, out var globalAngle, out var globalLayer, out var globalVelocity, out var globalAngularVelocity);
                e.RemoveParent();
                e.ReplacePosition(globalPosition);
                e.ReplaceDirection(globalAngle);
                e.ReplaceVelocity(globalVelocity);
                e.ReplaceAngularVelocity(globalAngularVelocity);
            }
        }
    }
}
