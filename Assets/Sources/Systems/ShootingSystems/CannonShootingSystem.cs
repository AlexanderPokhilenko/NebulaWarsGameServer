using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class CannonShootingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> shootingGroup;

    public CannonShootingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.TryingToShoot, GameMatcher.Cannon).NoneOf(GameMatcher.CannonCooldown);
        shootingGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in shootingGroup)
        {
            var bullet = e.cannon.bullet;
            var bulletEntity = bullet.CreateEntity(gameContext);
            // для быстрого перевода из локальной в глобальную системы координат
            bulletEntity.AddParent(e.id.value);
            bulletEntity.AddPosition(e.cannon.position);
            bulletEntity.AddDirection(0);
            bulletEntity.AddVelocity(Vector2.right * bullet.maxVelocity);
            bulletEntity.AddAngularVelocity(bullet.maxAngularVelocity);
            bulletEntity.ToGlobal(gameContext, out var globalPosition, out var globalAngle, out var layer, out var globalVelocity, out var globalAngularVelocity);
            bulletEntity.RemoveParent();
            // замена локальных координат на глобальные
            bulletEntity.ReplacePosition(globalPosition);
            bulletEntity.ReplaceDirection(globalAngle);
            bulletEntity.ReplaceVelocity(globalVelocity);
            bulletEntity.ReplaceAngularVelocity(globalAngularVelocity);
        }
    }
}