using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class CollisionFixedPenetrationDetectionSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;

    public CollisionFixedPenetrationDetectionSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CollisionVector);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollidable && entity.hasCollisionVector && entity.hasPosition && entity.isUnmovable && !entity.isPassingThrough && entity.hasParent;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            //TODO: учитывать массу и импульс
            if (e.TryGetFirstGameEntity(gameContext, part => !part.isUnmovable, out var parentPart))
            {
                if (parentPart.hasCollisionVector)
                {
                    var newCollisionVector = parentPart.collisionVector.value + e.collisionVector.value;
                    parentPart.ReplaceCollisionVector(newCollisionVector);
                }
                else
                {
                    parentPart.AddCollisionVector(e.collisionVector.value);
                }
            }
        }
    }
}