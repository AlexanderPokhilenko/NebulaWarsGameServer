using UnityEngine;
using UnityEngine.SceneManagement;

namespace SharedSimulationCode
{
    /// <summary>
    /// Создаёт обьект в физической сцене матча.
    /// </summary>
    public class PhysicsSpawner
    {
        private readonly Scene scene;
        
        public PhysicsSpawner(Scene scene)
        {
            this.scene = scene;
        }

        public void Ignore(Collider[] colliders1, Collider[] colliders2)
        {
            foreach (var collider1 in colliders1)
            {
                foreach (var collider2 in colliders2)
                {
                    Physics.IgnoreCollision(collider1, collider2);
                }
            }
        }
        
        public GameObject Spawn(GameObject prefab, Vector3 position)
        {
            GameObject go = Object.Instantiate(prefab, position, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(go, scene);
            return go;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject go = Object.Instantiate(prefab, position, rotation);
            SceneManager.MoveGameObjectToScene(go, scene);
            return go;
        }
    }
}