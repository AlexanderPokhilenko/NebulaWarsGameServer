using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "BaseObjects/Bullet", order = 56)]
public class BulletObject : MovableObject
{
    [Min(0)]
    public float damage;
    public bool isCollapses;
    [Min(0)]
    public float lifetime;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddDamage(damage);
        entity.isCollapses = isCollapses;
        entity.AddLifetime(lifetime);

        return entity;
    }
}
