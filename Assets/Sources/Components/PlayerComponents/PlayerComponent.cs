using Entitas;
using Entitas.CodeGeneration.Attributes;


[Game, Input]
public class PlayerComponent : IComponent
{
    [PrimaryEntityIndex]
    public int id;
}