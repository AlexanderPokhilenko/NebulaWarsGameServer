using UnityEngine;

public abstract class ActiveAbility : AbilityInfo
{
    [Min(0)]
    public float cooldown;

    protected virtual void AbilityAction(GameEntity executor, GameContext gameContext)
    {
        executor.AddAbilityCooldown(cooldown);
    }

    public override void AddAbilityToEntity(GameEntity entity)
    {
        entity.AddAbility(cooldown, AbilityAction);

        entity.AddAbilityCooldown(cooldown);
    }
}
