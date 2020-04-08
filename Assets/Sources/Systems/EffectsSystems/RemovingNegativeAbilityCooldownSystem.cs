using Entitas;
using System.Collections.Generic;

public class RemovingNegativeAbilityCooldownSystem : ReactiveSystem<GameEntity>
{

    public RemovingNegativeAbilityCooldownSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AbilityCooldown);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAbilityCooldown;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            if(e.abilityCooldown.value <= 0) e.RemoveAbilityCooldown();
        }
    }
}