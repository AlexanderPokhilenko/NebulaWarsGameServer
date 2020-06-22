using System.Collections.Generic;
using Entitas;

public class MovingCheckerSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private readonly IGroup<GameEntity> movingGroup;
    private readonly List<GameEntity> movingBuffer;
    private const int predictedCapacity = 512;

    public MovingCheckerSystem(Contexts contexts) : base(contexts.game)
    {
        movingGroup = contexts.game.GetGroup(GameMatcher.Moving);
        movingBuffer = new List<GameEntity>(predictedCapacity);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Position, GameMatcher.Direction));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDirection && entity.hasPosition && entity.hasViewType;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isMoving = true;
        }
    }

    public void Cleanup()
    {
        foreach (var e in movingGroup.GetEntities(movingBuffer))
        {
            e.isMoving = false;
        }
    }
}