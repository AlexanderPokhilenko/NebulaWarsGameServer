using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;

[Game]
public sealed class BonusAdderComponent : IComponent
{
    public BaseObject bonusObject;
    public float duration;
    public bool colliderInheritance;
}
