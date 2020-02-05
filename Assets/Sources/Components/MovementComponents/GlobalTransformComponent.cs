using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

[Game]
public sealed class GlobalTransformComponent : IComponent
{
    public Vector2 position;
    public float angle;
}
