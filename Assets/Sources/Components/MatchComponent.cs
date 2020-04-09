using Entitas;
using Entitas.CodeGeneration.Attributes;

[Input]
public sealed class PlayerExit : IComponent
{
    //TODO это очень опасно
    public int PlayerId;
}