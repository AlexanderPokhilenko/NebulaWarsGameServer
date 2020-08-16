using Entitas;
using Server.GameEngine.Chronometers;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Обновляет значение перезарядки
    /// </summary>
    public class CannonCooldownDecreasingSystem:IExecuteSystem
    {
        private readonly IGroup<GameEntity> cooldownGroup;

        public CannonCooldownDecreasingSystem(Contexts contexts)
        {
            cooldownGroup = contexts.game.GetGroup(GameMatcher.CannonCooldown);
        }
        
        public void Execute()
        {
            var entities = cooldownGroup.GetEntities();
            for (var index = 0; index < entities.Length; index++)
            {
                var entity = entities[index];
                float cooldownInSec = entity.cannonCooldown.value;
                if (cooldownInSec > 0)
                {
                    // Debug.LogError("Уменьшение времен перезарядки");
                    entity.cannonCooldown.value = cooldownInSec - Chronometer.DeltaTime;
                }
                else
                {
                    // Debug.LogError("Удаление времени перезарядки");
                    entity.RemoveCannonCooldown();
                }
            }
        }
    }
}