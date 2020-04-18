using Entitas;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class BotsMovingSystem : IInitializeSystem, IExecuteSystem
{
    private readonly System.Random random;
    private readonly List<GameEntity> buffer;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> botsGroup;
    private readonly IGroup<GameEntity> withCannonGroup;
    private readonly IGroup<GameEntity> withDamageGroup;
    private readonly IGroup<GameEntity> withBonusGroup;
    private const float zoneWarningDistance = 5f;
    private GameEntity zone;

    public BotsMovingSystem(Contexts contexts)
    {
        random = new System.Random();
        buffer = new List<GameEntity>();
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.CircleCollider, GameMatcher.Bot).NoneOf(GameMatcher.TargetMovingPoint);
        botsGroup = gameContext.GetGroup(matcher);
        withCannonGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Cannon, GameMatcher.Position, GameMatcher.Direction));
        withDamageGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Damage, GameMatcher.Position, GameMatcher.Direction));
        withBonusGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.ViewType).AnyOf(GameMatcher.BonusAdder, GameMatcher.ActionBonus));
    }

    public void Initialize()
    {
#if UNITY_EDITOR // Нужно для дебага, иначе будет падать
        if (!gameContext.hasZone)
        {
            zone = gameContext.CreateEntity();
            zone.AddPosition(Vector2.zero);
            zone.AddDirection(0f);
            zone.AddCircleCollider(float.PositiveInfinity);
            gameContext.SetZone(zone.id.value);
        }
#endif
        zone = gameContext.zone.GetZone(gameContext);
    }

    public void Execute()
    {
        foreach (var e in botsGroup.GetEntities(buffer))
        {
            var currentPosition = e.GetGlobalPositionVector2(gameContext);
            var zonePosition = zone.GetGlobalPositionVector2(gameContext);
            var zoneRadius = zone.circleCollider.radius;

            float maxRadius = e.circleCollider.radius + (float)random.NextDouble() * e.maxVelocity.value;
            var sqrMaxRadius = maxRadius * maxRadius;
            Vector2 targetPosition = currentPosition + maxRadius * CoordinatesExtensions.GetRandomUnitVector2();

            //if (e.hasTargetingParameters)
            //{
            //    maxRadius = e.targetingParameters.radius;
            //    targetPosition = Random.Range(e.circleCollider.radius, maxRadius) * CoordinatesExtensions.GetRandomUnitVector2();
            //    targetPosition += currentPosition;
            //}
            //else
            //{
            //    maxRadius = zoneRadius;
            //    targetPosition = Random.Range(e.circleCollider.radius, maxRadius) * CoordinatesExtensions.GetRandomUnitVector2();
            //    targetPosition += zonePosition;
            //}

            if (e.hasMaxHealthPoints)
            {
                var dangerAvoidanceVector = new Vector2(0f, 0f);
                // считаем здоровье, учитывая щиты и остальные объекты
                var fullHp = e.GetAllChildrenGameEntities(gameContext, c => c.hasHealthPoints).Sum(c => c.healthPoints.value);
                // может быть > 1, потому что считаем со щитами
                var healthPercentage = fullHp / e.maxHealthPoints.value;
                var combatPower = e.GetAllChildrenGameEntities(gameContext, c => c.hasCannon).Sum(GetEntityCombatPower);
                combatPower *= healthPercentage;

                var visionRadius = maxRadius + (e.hasTargetingParameters ? e.targetingParameters.radius : maxRadius);
                var sqrVisionRadius = visionRadius * visionRadius;

                dangerAvoidanceVector +=
                    GetAvoidanceVector(withCannonGroup, cE => GetEntityCombatPower(cE) / combatPower);

                dangerAvoidanceVector +=
                    GetAvoidanceVector(withDamageGroup, cE => GetEntityDamagePower(cE) / fullHp);

                if (e.isBonusPickable && healthPercentage < 1f)
                {
                    //TODO: учитывать мощность бонуса
                    var healthLackPercentage = 1f - healthPercentage;
                    dangerAvoidanceVector -= GetAvoidanceVector(withBonusGroup, cE => healthLackPercentage);
                }

                if (dangerAvoidanceVector.sqrMagnitude > sqrMaxRadius)
                {
                    targetPosition += Vector2.ClampMagnitude(dangerAvoidanceVector, maxRadius);
                }
                else
                {
                    targetPosition += dangerAvoidanceVector;
                }

                /*static*/ float GetEntityCombatPower(GameEntity combatEntity)
                {
                    var cannon = combatEntity.cannon;
                    var bullet = cannon.bullet;

                    var power = bullet.collisionDamage;
                    if (!bullet.isCollapses) power *= bullet.lifetime;
                    power /= cannon.cooldown;

                    return power;
                }

                /*static*/ float GetEntityDamagePower(GameEntity combatEntity)
                {
                    var damage = combatEntity.damage.value;

                    if (!combatEntity.isCollapses && combatEntity.TryGetFirstGameEntity(gameContext, p => p.hasLifetime, out var withLifetime))
                    {
                        damage *= withLifetime.lifetime.value;
                    }

                    return damage;
                }

                Vector2 GetAvoidanceVector(IGroup<GameEntity> dangerGroup, Func<GameEntity, float> calculatingFunc)
                {
                    var avoidanceVector = new Vector2(0f, 0f);
                    foreach (var danger in dangerGroup)
                    {
                        if(danger.GetGrandOwnerId(gameContext) == e.GetGrandOwnerId(gameContext)) continue;
                        var reverseDirection = targetPosition - danger.GetGlobalPositionVector2(gameContext);
                        var sqrReverseDirection = reverseDirection.sqrMagnitude;
                        if (sqrReverseDirection > sqrVisionRadius) continue;
                        var dangerPercentage = calculatingFunc(danger);
                        dangerPercentage *= 1f - sqrReverseDirection / sqrVisionRadius;
                        avoidanceVector += reverseDirection.normalized * dangerPercentage;
                    }

                    return avoidanceVector;
                }
            }


            var safeRadius = zoneRadius - zoneWarningDistance;

            if ((targetPosition - zonePosition).sqrMagnitude >= safeRadius * safeRadius)
            {
                var vectorToCenter = zonePosition - currentPosition;
                var randomLength = zoneWarningDistance + (float) random.NextDouble() * (maxRadius - zoneWarningDistance);
                var savingVector = randomLength * vectorToCenter.normalized;
                targetPosition += savingVector;
            }

            e.AddTargetMovingPoint(targetPosition);
        }
    }
}
