using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using Transform = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform;

[Game]
public sealed class GlobalTransformComponent : IComponent
{
    public Vector2 position;
    public float angle;

    public Transform GetTransform() => new Transform(position, angle);
}
