using Entitas;
using UnityEngine;

namespace SharedSimulationCode.Systems.Spawn
{
    /// <summary>
    /// Добавляет Force
    /// Удаляет force компонент
    /// </summary>
    public class SpawnForceSystem : IExecuteSystem, ICleanupSystem
    {
        private readonly IGroup<GameEntity> needSpawnForceGroup;

        public SpawnForceSystem(Contexts contexts)
        {
            needSpawnForceGroup = contexts.game
                .GetGroup(GameMatcher.AllOf(GameMatcher.Rigidbody, GameMatcher.SpawnForce));
        }
        
        public void Execute()
        {
            foreach (var entity in needSpawnForceGroup)
            {
                Rigidbody rigidbody = entity.rigidbody.value;
                var forceVector = entity.spawnForce.vector3;
                rigidbody.AddForce(forceVector, ForceMode.VelocityChange);
            }
        }

        public void Cleanup()
        {
            var entities = needSpawnForceGroup.GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                entity.RemoveSpawnForce();
            }
        }
    }
}