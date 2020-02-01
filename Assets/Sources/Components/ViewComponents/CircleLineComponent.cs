using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using UnityEngine;

[Serializable]
[Game]
public sealed class CircleLineComponent : IComponent
{
    public int numSegments;
    public float width;
    public Material material;
}