using UnityEngine;

public abstract class ActiveAbility : AbilityInfo
{
    [Min(0)]
    public float cooldown;

    protected virtual void AbilityAction(ServerGameEntity executor, ServerGameContext gameContext)
    {
        executor.AddAbilityCooldown(cooldown);
    }

    public override void AddAbilityToEntity(ServerGameEntity entity)
    {
        entity.AddAbility(cooldown, AbilityAction);

        entity.AddAbilityCooldown(cooldown);
    }
}
