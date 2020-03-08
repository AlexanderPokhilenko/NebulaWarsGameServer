using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using UnityEngine;


[Game]
public sealed class TargetMovingPointComponent : IComponent
{
    public Vector2 position;
}