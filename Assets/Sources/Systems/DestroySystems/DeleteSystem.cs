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
        deletingGroup = contexts.game.GetGroup(GameMatcher.NeededToDelete);
    }

    public void Execute()
    {
        //TODO: посмотреть, можно ли как-то удалять без использования массива
        foreach (var e in deletingGroup.GetEntities())
        {
            if (e.hasView)
            {
                var gameObject = e.view.gameObject;
                gameObject.Unlink();
                Object.Destroy(gameObject);
            }
            e.Destroy();
        }
    }
}
