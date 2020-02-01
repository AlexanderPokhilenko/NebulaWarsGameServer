using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using UnityEngine;

[Game]
public sealed class CannonComponent : IComponent
{
    public Vector2 position;
    public float cooldown;
    public BulletObject bullet;
}