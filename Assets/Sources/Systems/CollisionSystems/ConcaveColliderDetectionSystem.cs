using System.Collections.Generic;
using Code.Common;
using Entitas;

public class ConcaveColliderDetectionSystem : ReactiveSystem<GameEntity>
{
    private static readonly ILog Log = LogManager.CreateLogger(typeof(ConcaveColliderDetectionSystem));
    
    public ConcaveColliderDetectionSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Concave.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollidable && entity.isConcave && entity.hasPathCollider;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            //TODO: как-то обрабатывать вогнутые фигуры
            Log.Error($"Concave entity detected! {string.Join("; ", e.pathCollider.dots)}");
            e.isCollidable = false;
        }
    }
}