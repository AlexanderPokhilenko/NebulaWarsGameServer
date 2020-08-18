using Libraries.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SharedSimulationCode.Physics
{
    public class PhysicsRaycaster
    {
        private readonly Scene scene;
        private readonly PhysicsScene physicsScene;
        private readonly ILog log = LogManager.CreateLogger(typeof(PhysicsRaycaster));

        public PhysicsRaycaster(Scene scene)
        {
            this.scene = scene;
            physicsScene = scene.GetPhysicsScene();
        }
        
        public bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, out RaycastHit raycastHit)
        {
            return physicsScene.Raycast(origin, direction, out raycastHit, maxDistance);
        }
    }
}