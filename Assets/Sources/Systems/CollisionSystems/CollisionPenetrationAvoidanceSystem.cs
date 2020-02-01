using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class CollisionPenetrationAvoidanceSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;

    public CollisionPenetrationAvoidanceSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CollisionVector);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollidable && entity.hasCollisionVector && entity.hasPosition && !entity.isUnmovable && !entity.isPassingThrough;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            //TODO: учитывать массу и импульс
            var newPos = e.position.value + e.GetLocalVector(gameContext, e.collisionVector.value) / 2;
            e.ReplacePosition(newPos);
        }
    }
}