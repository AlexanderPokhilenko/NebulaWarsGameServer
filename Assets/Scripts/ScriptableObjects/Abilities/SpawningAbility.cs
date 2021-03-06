﻿using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawningAbility", menuName = "Abilities/SpawningAbility", order = 52)]
public class SpawningAbility : ActiveAbility
{
    public EntityCreatorObject ability;
    public bool spawnAsChild;
    public Vector2 position;
    public float angle;

    protected override void AbilityAction(GameEntity executor, GameContext gameContext)
    {
        base.AbilityAction(executor, gameContext);

        executor.ToGlobal(gameContext, out var newPosition, out var newAngle, out _, out _, out _);

        newPosition += position.GetRotated(newAngle);
        newAngle += angle;

        var entity = ability.CreateEntity(gameContext, newPosition, newAngle, executor.team.id);
        if(executor.hasSkin) executor.skin.value.AddSkin(entity, gameContext);
        if (executor.hasAttackIncreasing)
        {
            var attackIncreasing = executor.attackIncreasing.value;
            foreach (var child in entity.GetAllChildrenGameEntities(gameContext))
            {
                child.AddAttackIncreasing(attackIncreasing);
                if(child.hasDamage) child.ReplaceDamage(child.damage.value * attackIncreasing);
            }
        }

        entity.AddOwner(executor.GetGrandParent(gameContext).id.value);
        entity.AddGrandOwner(executor.GetGrandOwnerId(gameContext));

        if (spawnAsChild)
        {
            entity.AddParent(executor.id.value);
        }
    }
}