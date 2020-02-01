using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using UnityEngine;


[Game]
public sealed class AuraComponent : IComponent
{
    public float outerRadius;
    public float damage;
}