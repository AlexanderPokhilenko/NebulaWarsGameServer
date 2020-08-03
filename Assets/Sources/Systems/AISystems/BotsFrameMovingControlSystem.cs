using Entitas;
using System.Collections.Generic;
using Server.GameEngine;
using Server.GameEngine.Chronometers;
using UnityEngine;

public sealed class BotsFrameMovingControlSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;
    private const float framesCountMultiplier = 1.25f;

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
            var perFrameMovement = Chronometer.DeltaTime * maxVelocity;
            var framesCount = Mathf.FloorToInt(framesCountMultiplier * Mathf.Sqrt(pathVector.sqrMagnitude / (perFrameMovement * perFrameMovement)));

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
