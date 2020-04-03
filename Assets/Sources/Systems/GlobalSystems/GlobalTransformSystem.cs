using System;
using Entitas;
using log4net;

public sealed class GlobalTransformSystem : IExecuteSystem, ICleanupSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> positionedGroup;
    private static readonly ILog Log = LogManager.GetLogger(typeof(GlobalTransformSystem));

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
            else
            {
                Log.Error("Объект с id " + e.id.value + " не имел GlobalTransform!");
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
            }

            position += parent.globalTransform.position;
        }

        entity.AddGlobalTransform(position, angle);
    }
}
