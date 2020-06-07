using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPathCollider", menuName = "ColliderInfos/PathCollider", order = 53)]
public class PathColliderInfo : ColliderInfo
{
    public Vector2[] dots;
    public override void AddColliderToEntity(GameEntity entity)
    {
        entity.AddPathCollider(dots);
    }
}
