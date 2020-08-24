using System;
using Entitas;
using Entitas.Unity;
using Plugins.submodules.SharedCode.Physics;
using Plugins.submodules.SharedCode.Systems.Spawn;
using UnityEngine;

namespace Server.GameEngine
{
    public class AsteroidsSpawnerSystem : IExecuteSystem, ICleanupSystem
    {
        private readonly PhysicsSpawner physicsSpawner;
        private readonly PrefabsStorage prefabsStorage;
        private readonly IGroup<ServerGameEntity> spawnAsteroidGroup;

        public AsteroidsSpawnerSystem(Contexts contexts,
            PhysicsSpawner physicsSpawner, PrefabsStorage prefabsStorage)
        {
            this.physicsSpawner = physicsSpawner;
            this.prefabsStorage = prefabsStorage;
            spawnAsteroidGroup = contexts.serverGame.GetGroup(ServerGameMatcher.SpawnAsteroid);
        }

        public void Execute()
        {
            foreach (ServerGameEntity entity in spawnAsteroidGroup)
            {
                ViewTypeEnum viewType = entity.viewType.value;
                Vector3 spawnPosition = entity.spawnTransform.position;
                Quaternion spawnRotation = entity.spawnTransform.rotation;
                GameObject prefab = prefabsStorage.GetPrefab(viewType);
                GameObject go = physicsSpawner.Spawn(prefab, spawnPosition, spawnRotation);
                go.Link(entity);
                entity.AddTransform(go.transform);
                Rigidbody rigidbody = go.GetComponent<Rigidbody>();
                entity.AddRigidbody(rigidbody);
            }
        }

        public void Cleanup()
        {
            var entities = spawnAsteroidGroup.GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                entity.RemoveSpawnAsteroid();
                entity.RemoveSpawnTransform();
            }
        }
    }
}