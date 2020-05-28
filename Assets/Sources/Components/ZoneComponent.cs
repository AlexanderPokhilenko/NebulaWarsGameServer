using Entitas;
using Entitas.CodeGeneration.Attributes;


[Game, Unique]
public sealed class ZoneComponent : IComponent
{
    public ushort id;

    public GameEntity GetZone(GameContext gameContext) => gameContext.GetEntityWithId(id);
}