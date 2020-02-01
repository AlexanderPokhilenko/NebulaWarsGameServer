using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColliderInfo : ScriptableObject
{
    public abstract void AddColliderToEntity(GameEntity entity);
}
