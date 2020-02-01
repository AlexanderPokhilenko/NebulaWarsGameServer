using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class TargetShootingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> shootingGroup;

    public TargetShootingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Target, GameMatcher.Cannon).NoneOf(GameMatcher.CannonCooldown);
        shootingGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in shootingGroup)
        {
            var currentPosition = e.GetGlobalPositionVector2(gameContext);

            var target = gameContext.GetEntityWithId(e.target.id);
            var targetPosition = target.GetGlobalPositionVector2(gameContext);

            var direction = targetPosition - currentPosition;

            var localCannonTargetPosition = e.GetLocalRotatedVector(gameContext, direction) - e.cannon.position;

            var absProjectionY = Mathf.Abs(localCannonTargetPosition.y);

            if (absProjectionY <= target.circleCollider.radius) e.isTryingToShoot = true;
        }
    }
}
