using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class AddViewSystem : ReactiveSystem<GameEntity>
{
    readonly Transform _viewContainer = new GameObject("Game Views").transform;

    public AddViewSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        var matcher = GameMatcher.AnyOf(GameMatcher.Sprite, GameMatcher.CircleLine);
        return context.CreateCollector(matcher);
    }

    protected override bool Filter(GameEntity entity)
    {
        return (entity.hasSprite || entity.hasCircleLine) && !entity.hasView;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            GameObject go = new GameObject("Game View");
            go.transform.SetParent(_viewContainer, false);
            e.AddView(go);
            go.Link(e);
        }
    }
}