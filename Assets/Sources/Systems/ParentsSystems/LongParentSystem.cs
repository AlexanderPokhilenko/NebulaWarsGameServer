using Entitas;
using System.Collections.Generic;

public class LongParentSystem : ReactiveSystem<GameEntity>
{

    public LongParentSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Parent.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasParent;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            e.isLongParent = true;
        }
    }
}