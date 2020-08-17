using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Создаёт GameObject в нужной физической сцене.
    /// Очищает SpawnForce компонент
    /// </summary>
    public class WarshipsSpawnerSystem : IExecuteSystem, ICleanupSystem
    {
        private readonly PhysicsSpawner physicsSpawner;
        private readonly ViewTypePathStorage viewTypeStorage;
        private readonly IGroup<GameEntity> needSpawnWarships;

        public WarshipsSpawnerSystem(Contexts contexts, PhysicsSpawner physicsSpawner)
        {
            this.physicsSpawner = physicsSpawner;
            needSpawnWarships = contexts.game.GetGroup(GameMatcher
                .AllOf(GameMatcher.ViewType, GameMatcher.SpawnPoint, GameMatcher.SpawnWarship));
            viewTypeStorage = new ViewTypePathStorage();
        }
        
        public void Execute()
        {
            foreach (var entity in needSpawnWarships)
            {
                var viewType = entity.viewType.id;
                var spawnPosition = entity.spawnPoint.position;
                var spawnRotation = entity.spawnPoint.rotation;
                string path = viewTypeStorage.GetPath(viewType);
                GameObject prefab = Resources.Load<GameObject>(path);
                var go = physicsSpawner.Spawn(prefab, spawnPosition);

                go.Link(entity);
                go.transform.rotation = spawnRotation;
                entity.AddTransform(go.transform);
                var rigidbody = go.GetComponent<Rigidbody>();
                entity.AddRigidbody(rigidbody);
                List<Transform> shootingPoints = GetShootingPoints(go.transform);
                entity.AddShootingPoints(shootingPoints);
                Collider[] colliders = go.GetComponents<Collider>();
                entity.AddWarshipColliders(colliders);
            }
        }

        private List<Transform> GetShootingPoints(Transform parent)
        {
            string tag = "ShootPosition";
            List<Transform> result = parent.GetChildrenWithTag(tag);
            return result;
        }

        public void Cleanup()
        {
            var entities = needSpawnWarships.GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                entity.RemoveSpawnPoint();
            }
        }
    }
}