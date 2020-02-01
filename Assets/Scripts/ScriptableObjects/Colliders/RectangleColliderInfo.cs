using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRectangleCollider", menuName = "ColliderInfo/RectangleCollider", order = 52)]
public class RectangleColliderInfo : ColliderInfo
{
    [Min(0)]
    public float width;
    [Min(0)]
    public float height;

    public override void AddColliderToEntity(GameEntity entity)
    {
        entity.AddRectangleCollider(width, height);
    }
}
