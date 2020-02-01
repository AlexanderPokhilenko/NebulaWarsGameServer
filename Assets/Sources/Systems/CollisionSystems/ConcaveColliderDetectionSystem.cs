using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class ConcaveColliderDetectionSystem : ReactiveSystem<GameEntity>
{
    public ConcaveColliderDetectionSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Concave.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollidable && entity.isConcave && entity.hasPathCollider;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            //TODO: как-то обрабатывать вогнутые фигуры
            Debug.LogError($"Concave entity detected! {string.Join("; ", e.pathCollider.dots)}");
            e.isCollidable = false;
        }
    }
}