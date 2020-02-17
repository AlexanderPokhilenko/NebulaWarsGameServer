using System.Collections;
using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using UnityEngine;

public sealed class AuraDamageSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> auraGroup;
    private IGroup<GameEntity> damagableGroup;

    public AuraDamageSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Aura, GameMatcher.Position, GameMatcher.CircleCollider);
        auraGroup = gameContext.GetGroup(matcher);
        var damagableMatcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Collidable, GameMatcher.HealthPoints);
        damagableGroup = gameContext.GetGroup(damagableMatcher);
    }

    public void Execute()
    {
        foreach (var e in auraGroup)
        {
            var currentGlobalPosition = e.GetGlobalPositionVector2(gameContext);
            var fullRadius = e.circleCollider.radius + e.aura.outerRadius;
            var colliderSqrRadius = e.circleCollider.radius * e.circleCollider.radius;
            var fullSqrRadius = fullRadius * fullRadius;
            var currentDamage = e.aura.damage * Clock.deltaTime;
            foreach (var damagable in damagableGroup)
            {
                var damagableGlobalPosition = damagable.GetGlobalPositionVector2(gameContext);
                var direction = damagableGlobalPosition - currentGlobalPosition;
                var sqrDistance = direction.sqrMagnitude;
                if (sqrDistance <= fullSqrRadius && sqrDistance > colliderSqrRadius)
                {
                    damagable.ReplaceHealthPoints(damagable.healthPoints.value - currentDamage);
                }
            }
        }
    }
}
