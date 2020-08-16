using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace SharedSimulationCode
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
                foreach (var shootingPoint in shootingPoints)
                {
                    playerEntity.ReplaceCannonCooldown(1f);
                
                    //спавн пуль
                    var bulletEntity = gameContext.CreateEntity();
                    bulletEntity.AddDamage(10);
                    bulletEntity.AddViewType(ViewTypeId.DefaultShoot);
                    bulletEntity.isSpawnProjectile = true;
                    var position = shootingPoint.position;
                    Debug.LogError($"shootingPoint.position {position.x} {position.y} {position.z}");
                    Vector3 spawnPosition = warshipTransform.position + position;
                    bulletEntity.AddSpawnTransform(spawnPosition, warshipTransform.transform.rotation);
                    bulletEntity.AddSpawnForce(new Vector3(0, 0, 50));
                }
            }
        }
    }
}