using Entitas;
using System.Collections.Generic;
using Server.GameEngine;
using UnityEngine;

public sealed class BotsFrameMovingControlSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;
    public BotsFrameMovingControlSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TargetMovingPoint);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isBot && entity.hasPosition && entity.hasMaxVelocity;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (!e.hasTargetMovingPoint)
            {
                if(e.hasMovementFrames) e.RemoveMovementFrames();
                continue;
            }

            var pathVector = e.targetMovingPoint.position - e.GetGlobalPositionVector2(gameContext);
            var maxVelocity = e.maxVelocity.value;
            var perFrameMovement = Chronometer.GetMagicDich() * maxVelocity;
            var framesCount = Mathf.FloorToInt(Mathf.Sqrt(1.25f * pathVector.sqrMagnitude / (perFrameMovement * perFrameMovement)));

            if (e.hasMovementFrames)
            {
                e.ReplaceMovementFrames(framesCount);
            }
            else
            {
                e.AddMovementFrames(framesCount);
            }
        }
    }
}
