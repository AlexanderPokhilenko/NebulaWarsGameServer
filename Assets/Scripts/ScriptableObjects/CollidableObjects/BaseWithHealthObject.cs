using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBaseHealthObject", menuName = "BaseObjects/BaseHealthObject", order = 52)]
public class BaseWithHealthObject : BaseObject
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
