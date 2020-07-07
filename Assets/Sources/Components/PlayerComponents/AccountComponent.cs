using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class AccountComponent : IComponent
{
    [PrimaryEntityIndex]
    public int id;
}