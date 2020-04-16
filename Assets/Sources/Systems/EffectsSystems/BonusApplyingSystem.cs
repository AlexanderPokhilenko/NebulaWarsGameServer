using System;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using UnityEngine;

public class BonusApplyingSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;
    private const float colliderScalingCoefficient = 1.25f;
    private const float maxSameScaleDelta = 0.05f;

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
            if (addableBonus.hasLifetime)
            {
                addableBonus.ReplaceLifetime(e.bonusAdder.duration);
            }
            else
            {
                addableBonus.AddLifetime(e.bonusAdder.duration);
            }

            if (e.bonusAdder.colliderInheritance)
            {
                var targetRadius = pickablePart.circleCollider.radius * colliderScalingCoefficient;
                if (addableBonus.hasCircleCollider)
                {
                    if (Math.Abs(targetRadius - addableBonus.circleCollider.radius) > maxSameScaleDelta)
                    {
                        addableBonus.isNonstandardRadius = true;
                        addableBonus.AddTargetScaling(targetRadius);
                        var scalingTime = Mathf.Min(1f, Mathf.Max(0.05f * e.bonusAdder.duration, Chronometer.GetMagicDich()));
                        var scalingSpeed = (targetRadius - addableBonus.circleCollider.radius) / scalingTime;
                        addableBonus.AddCircleScaling(scalingSpeed);
                    }
                    else
                    {
                        addableBonus.ReplaceCircleCollider(targetRadius);
                    }
                }
                else
                {
                    addableBonus.isNonstandardRadius = true;
                    addableBonus.AddCircleCollider(targetRadius);
                }
            }

            Vector2 position;
            float angle;

            if (addableBonus.isParasite)
            {
                addableBonus.AddTarget(pickablePart.id.value);
                var parasiteGrandOwner = e.GetGrandOwnerId(gameContext);
                addableBonus.AddOwner(parasiteGrandOwner);
                addableBonus.AddGrandOwner(parasiteGrandOwner);
                e.ToGlobal(gameContext, out position, out angle, out _, out _, out _);
                addableBonus.ReplacePosition(position);
                addableBonus.ReplaceDirection(angle);
            }
            else
            {
                addableBonus.AddParent(pickablePart.id.value);
                addableBonus.AddOwner(pickablePart.GetGrandParent(gameContext).id.value);
                addableBonus.AddGrandOwner(pickablePart.GetGrandOwnerId(gameContext));
                addableBonus.isParentFixed = true;
                addableBonus.isParentDependent = true;
                addableBonus.isIgnoringParentCollision = true;
                addableBonus.ToGlobal(gameContext, out position, out angle, out _, out _, out _);
            }

            addableBonus.AddGlobalTransform(position, angle);
            e.isDestroyed = true;
        }
    }
}