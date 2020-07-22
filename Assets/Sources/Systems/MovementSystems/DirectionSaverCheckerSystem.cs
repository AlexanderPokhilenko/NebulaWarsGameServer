using System.Collections.Generic;
using Entitas;

public class DirectionSaverCheckerSystem : ReactiveSystem<GameEntity>
{
    public DirectionSaverCheckerSystem(Contexts contexts) : base(contexts.game)
    { }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.DirectionSaver, GameMatcher.DirectionTargeting).NoneOf(GameMatcher.DirectionTargetingShooting));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDirection && entity.hasDirectionSaver && !entity.isDirectionTargetingShooting;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.ReplaceDirectionTargeting(e.directionSaver.angle);
        }
    }
}