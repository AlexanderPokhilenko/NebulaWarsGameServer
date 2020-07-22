using System;
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
        var child = baseObject.CreateEntity(context, position, direction);
        child.AddParent(parent.id.value);
        child.isParentDependent = isParentDependent;
        child.isIgnoringParentCollision = isIgnoringParentCollision;
        child.isParentFixed = isParentFixed;
    }
}