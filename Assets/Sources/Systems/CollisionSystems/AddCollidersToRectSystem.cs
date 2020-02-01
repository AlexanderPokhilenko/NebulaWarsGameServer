using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class AddCollidersToRectSystem : ReactiveSystem<GameEntity>
{
    public AddCollidersToRectSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.RectangleCollider.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollidable && !entity.hasPathCollider;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var x = e.rectangleCollider.width / 2;
            var y = e.rectangleCollider.height / 2;
            var maxDist = Mathf.Sqrt(x * x + y * y);
            var dots = new[]
            {
                new Vector2(-x, -y),
                new Vector2(-x, y),
                new Vector2(x, y),
                new Vector2(x, -y)
            };

            var axises = new[]
            {
                new Vector2(1, 0),
                new Vector2(0, 1)
            };

            e.AddPathCollider(dots);
            e.AddCircleCollider(maxDist);
            e.AddNoncollinearAxises(axises);
        }
    }
}