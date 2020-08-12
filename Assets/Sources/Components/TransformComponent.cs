using Entitas;
using UnityEngine;


[Game]
public class TransformComponent:IComponent
{
    public Transform value;
}

[Game]
public class RigidbodyComponent:IComponent
{
    public Rigidbody value;
}
