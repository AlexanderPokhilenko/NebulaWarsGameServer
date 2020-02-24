﻿using UnityEngine;

[CreateAssetMenu(fileName = "NewMovableObject", menuName = "BaseObjects/MovableObject", order = 53)]
public class MovableObject : BaseObject
{
    [Min(0)]
    public float maxVelocity;
    [Min(0)]
    public float maxAngularVelocity;
    public bool isNotDecelerating;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddMaxVelocity(maxVelocity);
        entity.AddMaxAngularVelocity(maxAngularVelocity);
        entity.isNotDecelerating = isNotDecelerating;

        return entity;
    }
}
