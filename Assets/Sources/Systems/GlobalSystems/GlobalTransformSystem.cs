using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class GlobalTransformSystem : IExecuteSystem, ICleanupSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> positionedGroup;

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
                Debug.LogError("Объект с id " + e.id.value + " не имел GlobalTransform!");
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
            if (parentAngle != 0f)
            {
                position.Rotate(parentAngle);
                angle += parentAngle;
            }

            position += parent.globalTransform.position;
        }

        entity.AddGlobalTransform(position, angle);
    }
}
