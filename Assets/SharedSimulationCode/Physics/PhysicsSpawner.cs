using Libraries.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SharedSimulationCode.Physics
{
    /// <summary>
    /// Создаёт обьект в физической сцене матча.
    /// </summary>
    public class PhysicsSpawner
    {
        private readonly Scene scene;
        private readonly int ignoreRaycastLayerNum;
        private readonly ILog log = LogManager.CreateLogger(typeof(PhysicsSpawner));
        
        public PhysicsSpawner(Scene scene)
        {
            this.scene = scene;
            ignoreRaycastLayerNum = LayerMask.NameToLayer("Ignore Raycast");
        }

        public void Ignore(Collider[] colliders1, Collider[] colliders2)
        {
            //todo это чудо не работает
            // log.Debug($"colliders1.Length = {colliders1.Length}");
            // log.Debug($"colliders2.Length = {colliders2.Length}");
            foreach (var collider1 in colliders1)
            {
                foreach (var collider2 in colliders2)
                {
                    // log.Debug(collider1.name+" "+collider2.name);
                    UnityEngine.Physics.IgnoreCollision(collider1, collider2);
                }
            }
        }
        
        public GameObject Spawn(GameObject prefab, Vector3 position)
        {
            GameObject go = Object.Instantiate(prefab, position, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(go, scene);
            return go;
        }

        public GameObject SpawnProjectile(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject go = Object.Instantiate(prefab, position, rotation);
            go.layer = ignoreRaycastLayerNum;
            SceneManager.MoveGameObjectToScene(go, scene);
            return go;
        }
    }
}