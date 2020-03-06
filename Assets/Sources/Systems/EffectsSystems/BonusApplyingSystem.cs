using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class BonusApplyingSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;

    public BonusApplyingSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.BonusTarget.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasBonusAdder && entity.hasBonusTarget;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var pickablePart = gameContext.GetEntityWithId(e.bonusTarget.id);

            var addableBonus = e.bonusAdder.bonusObject.CreateEntity(gameContext, Vector2.zero, 0f);
            addableBonus.AddLifetime(e.bonusAdder.duration);
            if (e.bonusAdder.colliderInheritance)
            {
                if (addableBonus.hasCircleCollider)
                {
                    if (pickablePart.circleCollider.radius != addableBonus.circleCollider.radius)
                        addableBonus.isNonstandardRadius = true;
                    addableBonus.ReplaceCircleCollider(pickablePart.circleCollider.radius);
                }
                else
                {
                    addableBonus.isNonstandardRadius = true;
                    addableBonus.AddCircleCollider(pickablePart.circleCollider.radius);
                }
            }
            addableBonus.AddParent(pickablePart.id.value);
            addableBonus.AddOwner(pickablePart.GetGrandParent(gameContext).id.value);
            addableBonus.isParentFixed = true;
            addableBonus.isParentDependent = true;
            addableBonus.isIgnoringParentCollision = true;
            addableBonus.ToGlobal(gameContext, out var position, out var angle, out _, out _, out _);
            addableBonus.AddGlobalTransform(position, angle);
            e.isDestroyed = true;
        }
    }
}