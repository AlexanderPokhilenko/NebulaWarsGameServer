using Entitas;
using System.Collections.Generic;

public sealed class BotsOnHealthMoveChangingSystem : ReactiveSystem<GameEntity>
{
    public BotsOnHealthMoveChangingSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.HealthPoints);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isBot && entity.hasTargetMovingPoint;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.RemoveTargetMovingPoint();
        }
    }
}
