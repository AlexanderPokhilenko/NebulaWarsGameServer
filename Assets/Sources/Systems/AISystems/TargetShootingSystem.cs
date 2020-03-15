using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class TargetShootingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> shootingGroup;

    public TargetShootingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Target);
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

            var localTargetPosition = e.GetLocalRotatedVector(gameContext, direction);

            if(e.hasCannon) localTargetPosition -= e.cannon.position;

            var absProjectionY = Mathf.Abs(localTargetPosition.y);

            if (absProjectionY <= target.circleCollider.radius)
            {
                e.isTryingToShoot = true;
                var childrenWithCannon = e.GetAllChildrenGameEntities(gameContext, entity => entity.hasCannon);
                foreach (var childWithCannon in childrenWithCannon)
                {
                    childWithCannon.isTryingToShoot = true;
                }
            }
        }
    }
}
