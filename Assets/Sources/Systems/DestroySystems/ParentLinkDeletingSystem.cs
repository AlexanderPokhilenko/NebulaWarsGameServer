using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public sealed class ParentLinkDeletingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> withParentGroup;

    public ParentLinkDeletingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        withParentGroup = gameContext.GetGroup(GameMatcher.Parent);
    }

    public void Execute()
    {
        //TODO: посмотреть, можно ли как-то удалять без использования массива
        foreach (var e in withParentGroup.GetEntities())
        {
            var parent = gameContext.GetEntityWithId(e.parent.id);
            if (parent.isDestroyed)
            {
                if(e.isParentDependent) e.isDestroyed = true;
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
