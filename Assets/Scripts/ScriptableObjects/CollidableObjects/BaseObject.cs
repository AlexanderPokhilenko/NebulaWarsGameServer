using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBaseObject", menuName = "BaseObjects/BaseObject", order = 51)]
public class BaseObject : ScriptableObject
{
    public ViewTypeId typeId;
    public ColliderInfo colliderInfo;
    public CannonInfo cannonInfo;
    public bool isUnmovable;
    public bool isPassingThrough;
    [Min(0)]
    public float collisionDamage;
    public PartObject[] parts;

    public virtual GameEntity CreateEntity(GameContext context)
    {
        var entity = context.CreateEntity();
        if(typeId != ViewTypeId.Invisible) entity.AddViewType(typeId);
        if (colliderInfo != null)
        {
            entity.isCollidable = true;
            colliderInfo.AddColliderToEntity(entity);
        }

        cannonInfo?.AddCannonToEntity(entity);

        entity.isUnmovable = isUnmovable;
        entity.isPassingThrough = isPassingThrough;
        if(collisionDamage > 0) entity.AddDamage(collisionDamage);

        if (parts != null)
        {
            foreach (var part in parts)
            {
                part.AddPartToEntity(context, entity);
            }
        }

        return entity;
    }

    public GameEntity CreateEntity(GameContext context, Vector2 position, float angle)
    {
        var entity = CreateEntity(context);
        entity.AddPosition(position);
        entity.AddDirection(angle);
        return entity;
    }
}
