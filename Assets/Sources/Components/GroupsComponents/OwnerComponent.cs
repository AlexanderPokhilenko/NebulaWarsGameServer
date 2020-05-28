using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;


[Game]
public sealed class OwnerComponent : IComponent
{
    [EntityIndex]
    public ushort id;
}