using System.Collections.Generic;
using Entitas;

public class AbilityUsageRemoverSystem : ReactiveSystem<GameEntity>
{

    public AbilityUsageRemoverSystem(Contexts contexts) : base(contexts.game)
    { }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TryingToUseAbility.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isTryingToUseAbility;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isTryingToUseAbility = false;
        }
    }
}