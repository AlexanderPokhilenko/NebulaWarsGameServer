using System.Collections.Generic;
using Entitas;


[Game]
public sealed class DropComponent : IComponent
{
    public List<EntityCreatorObject> objects;
}