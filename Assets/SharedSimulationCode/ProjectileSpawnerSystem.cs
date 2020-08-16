using Entitas;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Создаёт game object для снарядов
    /// </summary>
    public class ProjectileSpawnerSystem :  IExecuteSystem, ICleanupSystem
    {
        private readonly PhysicsSpawner physicsSpawner;
        private readonly IGroup<GameEntity> needProjectileGroup;
        private readonly ProjectileViewTypePathStorage projectileViewTypePathStorage;

        public ProjectileSpawnerSystem(Contexts contexts, PhysicsSpawner physicsSpawner)
        {
            this.physicsSpawner = physicsSpawner;
            needProjectileGroup = contexts.game.GetGroup(GameMatcher
                .AllOf(GameMatcher.ViewType, GameMatcher.SpawnProjectile, GameMatcher.SpawnTransform));
            projectileViewTypePathStorage = new ProjectileViewTypePathStorage();
        }

        public void Execute()
        {
            foreach (var entity in needProjectileGroup)
            {
                // Debug.LogError("Создание снаряда");
                ViewTypeId viewType = entity.viewType.id;
                Vector3 spawnPosition = entity.spawnTransform.position;
                Quaternion spawnRotation = entity.spawnTransform.rotation;
                string path = projectileViewTypePathStorage.GetPath(viewType);
                GameObject prefab = Resources.Load<GameObject>(path);
                GameObject go = physicsSpawner.Spawn(prefab, spawnPosition, spawnRotation);
                entity.AddTransform(go.transform);
                Rigidbody rigidbody = go.GetComponent<Rigidbody>();
                entity.AddRigidbody(rigidbody);
            }
        }

        public void Cleanup()
        {
            var entities = needProjectileGroup.GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                entity.isSpawnProjectile = false;
                entity.RemoveSpawnTransform();
            }
        }
    }
}