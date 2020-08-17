using System.Collections.Generic;
using Entitas;
using UnityEngine;

[Game]
public class SpawnTransformComponent:IComponent
{
    public Transform transform;
}


[Game]
public class ParentWarshipComponent:IComponent
{
    public GameEntity entity;
}

[Game]
public class WarshipCollidersComponent:IComponent
{
    public Collider[] colliders;
}

[Game]
public class SpawnPointComponent:IComponent
{
    public Vector3 position;
    public Quaternion rotation;
}

[Game]
public class SpawnForceComponent:IComponent
{
    public Vector3 vector3;
}

[Game]
public class ShootingPointsComponent:IComponent
{
    public List<Transform> values;
}

[Game]
public class SpawnWarshipComponent:IComponent
{
    
}

[Game]
public class SpawnProjectileComponent:IComponent
{
    
}

[Game]
public class HitComponent:IComponent
{
    public GameEntity projectileEntity;
    public GameEntity warshipEntity;
}

