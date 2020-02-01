using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBaseObject", menuName = "BaseObjects/BaseObject", order = 51)]
public class BaseObject : ScriptableObject
{
    public Sprite sprite;
    public CircleLineComponent circleLine = null;
    public AnimatorController controller;
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
        if (sprite != null) entity.AddSprite(sprite);
        if(circleLine != null && circleLine.numSegments >= 3 && circleLine.width > 0 && circleLine.material != null)
            entity.AddCircleLine(circleLine.numSegments, circleLine.width, circleLine.material);
        if (controller != null) entity.AddAnimatorController(controller);
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
