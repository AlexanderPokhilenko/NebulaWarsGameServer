using System.Collections.Generic;
using Entitas;
using UnityEngine;

[Game]
public class SpawnPositionComponent:IComponent
{
    public Vector3 vector3;
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
public class SpawnBulletComponent:IComponent
{
    
}

