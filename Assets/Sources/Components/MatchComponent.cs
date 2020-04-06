using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique]
public sealed class MatchComponent : IComponent
{
    public int MatchId;
}


[Input]
public sealed class PlayerExit : IComponent
{
    //TODO это очень опасно
    public int PlayerId;
}