using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public sealed class DeleteSystem : IExecuteSystem
{
    private IGroup<GameEntity> deletingGroup;

    public DeleteSystem(Contexts contexts)
    {
        deletingGroup = contexts.game.GetGroup(GameMatcher.Destroyed);
    }

    public void Execute()
    {
        //TODO: посмотреть, можно ли как-то удалять без использования массива
        foreach (var e in deletingGroup.GetEntities())
        {
            e.Destroy();
        }
    }
}
