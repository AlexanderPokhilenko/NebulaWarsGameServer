using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public sealed class TargetLinkDeletingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> withTargetGroup;

    public TargetLinkDeletingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        withTargetGroup = gameContext.GetGroup(GameMatcher.Target);
    }

    public void Execute()
    {
        //TODO: посмотреть, можно ли как-то удалять без использования массива
        foreach (var e in withTargetGroup.GetEntities())
        {
            var target = gameContext.GetEntityWithId(e.target.id);
            if (target.isDestroyed)
            {
                e.RemoveTarget();
            }
        }
    }
}
