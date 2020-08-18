using Entitas;
using UnityEngine;


[Input]
public class MovementComponent : IComponent
{
    public Vector2 value;
}

[Input]
public class CreationTickNumberComponent : IComponent
{
    public int value;
}


[Game]
public class TickNumberComponent : IComponent
{
    public int value;
}

