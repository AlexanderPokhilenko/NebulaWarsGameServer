using Entitas.VisualDebugging.Unity;
using UnityEngine;

namespace SharedSimulationCode.Physics
{
    public class PhysicsDestroyer
    { 
        public void Destroy(GameObject gameObject)
        {
            gameObject.DestroyGameObject();
        }
    }
}