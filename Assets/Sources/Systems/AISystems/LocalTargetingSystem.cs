using Entitas;
using System.Collections.Generic;

public sealed class LocalTargetingSystem : IExecuteSystem, ICleanupSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> targetingGroup;
    private const int PredictedCapacity = 32;
    private readonly List<GameEntity> buffer = new List<GameEntity>(PredictedCapacity);

    public LocalTargetingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Direction,
                GameMatcher.LocalDirectionTargeting)
            .NoneOf(GameMatcher.DirectionTargeting,
                GameMatcher.Target,
                GameMatcher.DirectionSaver);
        targetingGroup = gameContext.GetGroup(matcher);
    }

    public void Cleanup()
    {
        foreach (var e in buffer)
        {
            if (e == null || !e.hasDirectionTargeting) continue;
            e.RemoveDirectionTargeting();
        }
    }

    public void Execute()
    {
        foreach (var e in targetingGroup.GetEntities(buffer))
        {
            var currentDeltaAngle = e.GetGlobalAngle(gameContext) - e.direction.angle;

            var targetAngle = e.localDirectionTargeting.angle + currentDeltaAngle;

            if (targetAngle >= 360f)
            {
                targetAngle -= 360f;
            }
            else if (targetAngle < 0f)
            {
                targetAngle += 360f;
            }

            e.AddDirectionTargeting(targetAngle);
        }
    }
}