using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "BaseObjects/Bullet", order = 56)]
public class BulletObject : MovableObject
{
    public bool isCollapses;
    [Min(0)]
    public float lifetime;
    public bool detachable = true;
    public bool parentDependent;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.isCollapses = isCollapses;
        entity.AddLifetime(lifetime);
        if (!detachable)
        {
            entity.isParentFixed = true;
            entity.isIgnoringParentCollision = true;
            entity.isParentDependent = parentDependent;
        }

        return entity;
    }
}
