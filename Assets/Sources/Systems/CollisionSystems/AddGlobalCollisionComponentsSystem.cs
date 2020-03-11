using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class AddGlobalCollisionComponentsSystem : ReactiveSystem<GameEntity>
{
    private const float sameAngleDelta = 0.5f;
    private readonly GameContext gameContext;

    public AddGlobalCollisionComponentsSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.PathCollider.Added(), GameMatcher.NoncollinearAxises.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPathCollider && entity.hasNoncollinearAxises && !entity.hasGlobalPathCollider && !entity.hasGlobalNoncollinearAxises;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var angle = e.GetGlobalAngle(gameContext);
            if (Math.Abs(angle) > sameAngleDelta)
            {
                e.AddGlobalPathCollider(CoordinatesExtensions.GetRotatedVectors(e.pathCollider.dots, e.direction.angle));
                e.AddGlobalNoncollinearAxises(CoordinatesExtensions.GetRotatedVectors(e.noncollinearAxises.vectors, e.direction.angle));
            }
            else
            {
                e.AddGlobalPathCollider((Vector2[])e.pathCollider.dots.Clone());
                e.AddGlobalNoncollinearAxises((Vector2[])e.noncollinearAxises.vectors.Clone());
            }
        }
    }
}