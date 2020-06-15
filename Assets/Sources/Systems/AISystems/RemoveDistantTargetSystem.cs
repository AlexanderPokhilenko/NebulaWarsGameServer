using System.Collections.Generic;
using Entitas;

public sealed class RemoveDistantTargetSystem : ICleanupSystem
{
    private readonly List<GameEntity> buffer;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> targetingGroup;

    public RemoveDistantTargetSystem(Contexts contexts)
    {
        buffer = new List<GameEntity>();
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.TargetingParameters, 
            GameMatcher.Target);
        targetingGroup = gameContext.GetGroup(matcher);
    }

    public void Cleanup()
    {
        foreach (var e in targetingGroup.GetEntities(buffer))
        {
            var target = gameContext.GetEntityWithId(e.target.id);
            var targetPosition = target.GetGlobalPositionVector2(gameContext);
            var currentPosition = e.GetGlobalPositionVector2(gameContext);
            var targetingRadius = e.targetingParameters.radius;
            var direction = targetPosition - currentPosition;
            if (direction.sqrMagnitude > targetingRadius * targetingRadius)
            {
                e.RemoveTarget();
                if(e.hasDirectionTargeting) e.RemoveDirectionTargeting();
                if(e.hasAngularVelocity) e.RemoveAngularVelocity();
                e.isDirectionTargetingShooting = false;
            }
        }
    }
}
