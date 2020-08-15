using Entitas;
using Server.GameEngine.Chronometers;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Создаёт сущности для пуль/снарядов обычной атаки
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
                    if (playerEntity.cannonCooldown.value > 0)
                    {
                        playerEntity.cannonCooldown.value = playerEntity.cannonCooldown.value - Chronometer.DeltaTime; 
                        continue;    
                    }
                }
                
                float attackStickDirection = inputEntity.attack.direction;
                if (float.IsNaN(attackStickDirection))
                {
                    continue;
                }

                playerEntity.ReplaceCannonCooldown(1f);
                
                //спавн пуль
                var bulletEntity = gameContext.CreateEntity();
                bulletEntity.AddDamage(10);
                bulletEntity.AddViewType(ViewTypeId.DefaultShoot);
                bulletEntity.AddSpawnPosition(new Vector3(0, 0, 0));
                bulletEntity.AddSpawnForce(new Vector3(0, 0, 20));
            }
        }
    }
}