using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace SharedSimulationCode.Systems.InputHandling
{
    /// <summary>
    /// Создаёт сущности для пуль/снарядов обычной атаки по вводу игрока
    /// </summary>
    public class ShootingSystem : IExecuteSystem
    {
        private readonly GameContext gameContext;
        private readonly IGroup<InputEntity> attackGroup;

        public ShootingSystem(Contexts contexts)
        {
            attackGroup = contexts.input
                .GetGroup(InputMatcher.AllOf(InputMatcher.Attack, InputMatcher.Player));
            gameContext = contexts.game;
        }

        public void Execute()
        {
            foreach (var inputEntity in attackGroup)
            {
                ushort playerId = inputEntity.player.id;
                GameEntity playerEntity = gameContext.GetEntityWithPlayer(playerId);

                if (playerEntity.hasCannonCooldown)
                {
                    // Debug.LogError("перезарядка");
                    continue;
                }

                float attackStickDirection = inputEntity.attack.direction;
                if (float.IsNaN(attackStickDirection))
                {
                    // Debug.LogError("нет направления атаки");
                    continue;
                }

                if (!playerEntity.hasShootingPoints)
                {
                    Debug.LogError("Если есть Attack то должен быть ShootingPoints");
                    continue;
                }

                //выстрел
                // Debug.LogError("выстрел");
                var warshipTransform = playerEntity.transform.value;
                List<Transform> shootingPoints = playerEntity.shootingPoints.values;
                playerEntity.ReplaceCannonCooldown(0.5f);
                foreach (var shootingTransform in shootingPoints)
                {
                
                    //спавн пуль
                    var projectileEntity = gameContext.CreateEntity();
                    projectileEntity.AddDamage(10);
                    projectileEntity.AddViewType(ViewTypeId.DefaultShoot);
                    projectileEntity.isSpawnProjectile = true;
                    var position = shootingTransform.position;
                    // Debug.LogError($"shootingPoint.position {position.x} {position.y} {position.z}");
                    Vector3 spawnPosition = warshipTransform.localPosition + position;
                    projectileEntity.AddSpawnTransform(shootingTransform);
                    var direction = shootingTransform.transform.rotation *Vector3.forward;
                    projectileEntity.AddSpawnForce(direction.normalized * 20f);
                    projectileEntity.AddParentWarship(playerEntity);
                }
            }
        }
    }
}