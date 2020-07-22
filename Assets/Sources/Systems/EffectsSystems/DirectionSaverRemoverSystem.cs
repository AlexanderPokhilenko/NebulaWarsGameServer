using System.Collections.Generic;
using Entitas;

public sealed class DirectionSaverRemoverSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> directionSaverGroup;
    private const int PredictedCapacity = 50;
    private readonly List<GameEntity> buffer = new List<GameEntity>(PredictedCapacity);

    public DirectionSaverRemoverSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        directionSaverGroup = gameContext.GetGroup(GameMatcher.DirectionSaver);
    }

    public void Execute()
    {
        foreach (var e in directionSaverGroup.GetEntities(buffer))
        {
            if(e.directionSaver.time <= 0f) e.RemoveDirectionSaver();
        }
    }
}