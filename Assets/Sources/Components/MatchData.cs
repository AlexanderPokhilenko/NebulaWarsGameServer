using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique]
public sealed class MatchData : IComponent
{
    public int MatchId;

    // public GameEntity GetZone(GameContext gameContext) => gameContext.GetEntityWithId(id);
}