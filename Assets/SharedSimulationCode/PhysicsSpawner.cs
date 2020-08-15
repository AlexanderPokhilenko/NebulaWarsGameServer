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