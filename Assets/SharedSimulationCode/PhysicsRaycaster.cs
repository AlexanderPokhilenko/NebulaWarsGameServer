using UnityEngine;

namespace SharedSimulationCode
{
    public class PhysicsRaycaster
    {
        private readonly PhysicsScene physicsScene;

        public PhysicsRaycaster(PhysicsScene physicsScene)
        {
            this.physicsScene = physicsScene;
        }
        
        public RaycastHit[] Raycast(Vector3 origin, Vector3 direction, float maxDistance, RaycastHit[] raycastHits)
        {
            int count =  physicsScene.Raycast(origin, direction,raycastHits, maxDistance);
            return raycastHits;
        }
    }
}