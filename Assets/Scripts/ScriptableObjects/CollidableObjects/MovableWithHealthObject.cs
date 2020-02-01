using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMovableHealthObject", menuName = "BaseObjects/MovableHealthObject", order = 54)]
public class MovableWithHealthObject : MovableObject
{
    [Min(0)]
    public float maxHealthPoints;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddHealthPoints(maxHealthPoints);

        return entity;
    }
}
