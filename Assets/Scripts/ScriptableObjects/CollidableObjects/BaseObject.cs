using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBaseObject", menuName = "BaseObjects/BaseObject", order = 51)]
public class BaseObject : ScriptableObject
{
    //TODO: хранить ссылку / ID визуального объекта (Sprite, AnimatorController и CircleLineComponent)
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
}
