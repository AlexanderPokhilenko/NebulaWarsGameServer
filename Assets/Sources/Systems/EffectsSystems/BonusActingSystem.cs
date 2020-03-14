using Entitas;
using System.Collections.Generic;

public class BonusActingSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;

    public BonusActingSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.BonusTarget.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasActionBonus && entity.hasBonusTarget;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var pickablePart = gameContext.GetEntityWithId(e.bonusTarget.id);

            e.actionBonus.action(pickablePart);

            e.isDestroyed = true;
        }
    }
}