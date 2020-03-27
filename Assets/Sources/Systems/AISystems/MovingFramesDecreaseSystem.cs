using System.Collections.Generic;
using Entitas;

public sealed class MovingFramesDecreaseSystem : IExecuteSystem
{
    private readonly List<GameEntity> buffer;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> framesGroup;
    private readonly int predictedBotsCount = 10;

    public MovingFramesDecreaseSystem(Contexts contexts)
    {
        buffer = new List<GameEntity>(predictedBotsCount);
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Bot, GameMatcher.MovementFrames, GameMatcher.TargetMovingPoint);
        framesGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in framesGroup.GetEntities(buffer))
        {
            e.ReplaceMovementFrames(e.movementFrames.value - 1);
            if(e.movementFrames.value <= 0 ) e.RemoveTargetMovingPoint();
        }
    }
}
