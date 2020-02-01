using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public class PartObject
{
    public BaseObject baseObject;
    public Vector2 position;
    public float direction;
    public bool isParentDependent;
    public bool isIgnoringParentCollision;
    public bool isParentFixed = true;

    public void AddPartToEntity(GameContext context, GameEntity parent)
    {
        var child = baseObject.CreateEntity(context);
        child.AddParent(parent.id.value);
        child.AddPosition(position);
        child.AddDirection(direction);
        child.isParentDependent = isParentDependent;
        child.isIgnoringParentCollision = isIgnoringParentCollision;
        child.isParentFixed = isParentFixed;
    }
}
