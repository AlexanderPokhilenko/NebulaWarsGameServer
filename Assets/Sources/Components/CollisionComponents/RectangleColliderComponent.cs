using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;

[Game]
public sealed class RectangleColliderComponent : IComponent
{
    public float width;
    public float height;
}
