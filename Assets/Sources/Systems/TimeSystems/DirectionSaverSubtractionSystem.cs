using Entitas;
using Server.GameEngine;
using Server.GameEngine.Chronometers;

public sealed class DirectionSaverSubtractionSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> directionSaverGroup;

    public DirectionSaverSubtractionSystem(Contexts contexts)
    {
        directionSaverGroup = contexts.game.GetGroup(GameMatcher.DirectionSaver);
    }

    public void Execute()
    {
        foreach (var e in directionSaverGroup)
        {
            e.directionSaver.time -= Chronometer.DeltaTime;
        }
    }
}