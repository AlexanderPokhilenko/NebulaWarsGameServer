using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFlameCircle", menuName = "BaseObjects/FlameCircle", order = 58)]
public class FlameCircleObject : AuraObject
{
    public float scalingSpeed = -1.0f;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddCircleScaling(scalingSpeed);

        return entity;
    }
}
