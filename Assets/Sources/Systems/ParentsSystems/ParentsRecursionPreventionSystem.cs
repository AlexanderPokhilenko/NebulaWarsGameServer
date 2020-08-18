using System.Collections.Generic;
using Code.Common;
using Entitas;
using Libraries.Logger;

public class ParentsRecursionPreventionSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;
    private static readonly ILog Log = LogManager.CreateLogger(typeof(ParentsRecursionPreventionSystem));
    

    public ParentsRecursionPreventionSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Parent);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasParent;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var entityId = e.id.value;
            var firstParent = e;
            while (firstParent.hasParent)
            {
                firstParent = gameContext.GetEntityWithId(firstParent.parent.id);
                if (firstParent.id.value == entityId)
                {
                    Log.Warn("Parent recursion detected for entity with id " + entityId);
                    e.RemoveParent();
                    break;
                }
            }
        }
    }
}