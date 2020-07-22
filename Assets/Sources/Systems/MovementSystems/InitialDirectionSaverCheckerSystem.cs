using System.Collections.Generic;
using Entitas;

public class InitialDirectionSaverCheckerSystem : ReactiveSystem<GameEntity>
{
    public InitialDirectionSaverCheckerSystem(Contexts contexts) : base(contexts.game)
    { }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Direction, GameMatcher.InitialDirectionSaver).NoneOf(GameMatcher.DirectionTargeting, GameMatcher.DirectionSaver));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDirection && entity.hasInitialDirectionSaver && !entity.hasDirectionTargeting && !entity.hasDirectionSaver;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.ReplaceLocalDirectionTargeting(e.initialDirectionSaver.angle);
        }
    }
}