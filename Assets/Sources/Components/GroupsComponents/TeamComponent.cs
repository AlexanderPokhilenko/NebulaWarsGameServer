using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public sealed class TeamComponent : IComponent
{
    [EntityIndex]
    public byte id;
}