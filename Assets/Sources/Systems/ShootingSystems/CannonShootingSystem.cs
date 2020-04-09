using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class CannonShootingSystem : IExecuteSystem
{
    private readonly System.Random random;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> shootingGroup;

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
            var bulletEntity = ShootBullet(e, gameContext, bullet, e.cannon.position);

            if (!bulletEntity.hasChaser && bullet.maxAngularVelocity > 0f)
            {
                bulletEntity.ReplaceAngularVelocity(bullet.maxAngularVelocity * random.Next(-1, 2));
            }
        }
    }

    public static GameEntity ShootBullet(GameEntity shooter, GameContext gameContext, BulletObject bullet, Vector2 bulletOffset)
    {
        var bulletEntity = bullet.CreateEntity(gameContext);
        bulletEntity.AddOwner(shooter.GetGrandParent(gameContext).id.value);
        bulletEntity.AddGrandOwner(shooter.GetGrandOwnerId(gameContext));
        var bulletDeltaSize = bulletEntity.hasCircleCollider ? bulletEntity.circleCollider.radius :
            bulletEntity.hasRectangleCollider ? bulletEntity.rectangleCollider.width / 2 :
            bulletEntity.hasPathCollider ? -1 * bulletEntity.pathCollider.dots.Min(d => d.x) :
            throw new NotSupportedException("Ошибка вычисления размера снаряда. Вероятно, использовался неизвестный коллайдер.");
        // для быстрого перевода из локальной в глобальную системы координат
        bulletEntity.AddParent(shooter.id.value);
        bulletEntity.AddPosition(bulletOffset + Vector2.right * bulletDeltaSize);
        bulletEntity.AddDirection(0);
        bulletEntity.AddVelocity(Vector2.right * bullet.maxVelocity);
        bulletEntity.AddAngularVelocity(0f);

        if (bulletEntity.hasChaser)
        {
            if (shooter.hasTargetingParameters && !bulletEntity.hasTargetingParameters)
            {
                bulletEntity.AddTargetingParameters(shooter.targetingParameters.angularTargeting, shooter.targetingParameters.radius, shooter.targetingParameters.onlyPlayerTargeting);
            }
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

            if (!bulletEntity.hasChaser)
            {
                if (bullet.maxVelocity * bullet.maxVelocity < globalVelocity.sqrMagnitude)
                {
                    bulletEntity.ReplaceMaxVelocity(globalVelocity.magnitude);
                }

                var absGlobalAngularVelocity = Mathf.Abs(globalAngularVelocity);
                if (bullet.maxAngularVelocity < absGlobalAngularVelocity)
                {
                    bulletEntity.ReplaceMaxVelocity(absGlobalAngularVelocity);
                }
            }
        }

        bulletEntity.AddGlobalTransform(globalPosition, globalAngle);

        return bulletEntity;
    }
}