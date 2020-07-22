using System.Collections.Generic;
using Entitas;

public class AbilityUsingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> abilityUsingGroup;
    private const int PredictedCapacity = 10;
    private readonly List<GameEntity> buffer = new List<GameEntity>(PredictedCapacity);

    public AbilityUsingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.TryingToUseAbility, GameMatcher.Ability).NoneOf(GameMatcher.AbilityCooldown);
        abilityUsingGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in abilityUsingGroup.GetEntities(buffer))
        {
            e.ability.action(e, gameContext);
        }
    }
}