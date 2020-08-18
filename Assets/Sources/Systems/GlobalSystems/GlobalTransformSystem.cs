using System;
using Entitas;
using Libraries.Logger;

public sealed class GlobalTransformSystem : IExecuteSystem, ICleanupSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> positionedGroup;
    private static readonly ILog Log = LogManager.CreateLogger(typeof(GlobalTransformSystem));

    public GlobalTransformSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        positionedGroup = gameContext.GetGroup(GameMatcher.Position);
    }

    public void Execute()
    {
        foreach (var e in positionedGroup)
        {
            if(e.hasGlobalTransform) continue;
            AddGlobalTransform(e);
        }
    }

    public void Cleanup()
    {
        foreach (var e in positionedGroup)
        {
            if (e.hasGlobalTransform)
            {
                e.RemoveGlobalTransform();
            }
        }
    }

    private void AddGlobalTransform(GameEntity entity)
    {
        var position = entity.position.value;
        var angle = entity.hasDirection ? entity.direction.angle : 0f;

        if (entity.hasParent)
        {
            var parent = gameContext.GetEntityWithId(entity.parent.id);
            if (!parent.hasGlobalTransform) AddGlobalTransform(parent);

            var parentAngle = parent.globalTransform.angle;
            if (Math.Abs(parentAngle) > 0.001)
            {
                position.Rotate(parentAngle);
                angle += parentAngle;
                // Кажется, одного отнимания будет достаточно
                if (angle >= 360f) angle -= 360f;
            }

            position += parent.globalTransform.position;
        }

        entity.AddGlobalTransform(position, angle);
    }
}
