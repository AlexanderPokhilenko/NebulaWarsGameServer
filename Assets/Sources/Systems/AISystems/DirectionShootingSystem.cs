using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class DirectionShootingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> shootingGroup;
    private const float attackDelta = 2.5f;

    public DirectionShootingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Direction, GameMatcher.DirectionTargeting, GameMatcher.DirectionTargetingShooting).NoneOf(GameMatcher.Target);
        shootingGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in shootingGroup)
        {
            if (Mathf.Abs(e.GetGlobalAngle(gameContext) - e.directionTargeting.angle) > attackDelta) continue;
            e.isTryingToShoot = true;
            var childrenWithCannon = e.GetAllChildrenGameEntities(gameContext, entity => entity.hasCannon);
            foreach (var childWithCannon in childrenWithCannon)
            {
                childWithCannon.isTryingToShoot = true;
            }
        }
    }
}
