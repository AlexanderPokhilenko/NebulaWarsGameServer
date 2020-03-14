using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class CannonShootingSystem : IExecuteSystem
{
    private readonly System.Random random;
    private readonly GameContext gameContext;
    private IGroup<GameEntity> shootingGroup;

    public CannonShootingSystem(Contexts contexts)
    {
        random = new System.Random();
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
            bulletEntity.AddOwner(e.GetGrandParent(gameContext).id.value);
            bulletEntity.AddGrandOwner(e.GetGrandOwnerId(gameContext));
            var bulletDeltaSize = bulletEntity.hasCircleCollider ? bulletEntity.circleCollider.radius :
                bulletEntity.hasRectangleCollider ? bulletEntity.rectangleCollider.width / 2 :
                bulletEntity.hasPathCollider ? -1 * bulletEntity.pathCollider.dots.Min(d => d.x) :
                throw new NotSupportedException("Ошибка вычисления размера снаряда. Вероятно, использовался неизвестный коллайдер.");
            // для быстрого перевода из локальной в глобальную системы координат
            bulletEntity.AddParent(e.id.value);
            bulletEntity.AddPosition(e.cannon.position + Vector2.right * bulletDeltaSize);
            bulletEntity.AddDirection(0);
            bulletEntity.AddVelocity(Vector2.right * bullet.maxVelocity);

            if (bulletEntity.hasChaser)
            {
                if (e.hasTargetingParameters && !bulletEntity.hasTargetingParameters)
                {
                    bulletEntity.AddTargetingParameters(e.targetingParameters.angularTargeting, e.targetingParameters.radius, e.targetingParameters.onlyPlayerTargeting);
                }
            }
            else
            {
                bulletEntity.AddAngularVelocity(bullet.maxAngularVelocity * random.Next(-1, 2));
            }

            bulletEntity.ToGlobal(gameContext, out var globalPosition, out var globalAngle, out var layer, out var globalVelocity, out var globalAngularVelocity);
            if (bullet.detachable)
            {
                bulletEntity.RemoveParent();
                // замена локальных координат на глобальные
                bulletEntity.ReplacePosition(globalPosition);
                bulletEntity.ReplaceDirection(globalAngle);
                bulletEntity.ReplaceVelocity(globalVelocity);
                bulletEntity.ReplaceAngularVelocity(globalAngularVelocity);
            }
            bulletEntity.AddGlobalTransform(globalPosition, globalAngle);
            
        }
    }
}