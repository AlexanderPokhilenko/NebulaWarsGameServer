using Entitas.VisualDebugging.Unity;
using UnityEngine;

namespace SharedSimulationCode
{
    public class PhysicsDestroyer
    { 
        public void Destroy(GameObject gameObject)
        {
            gameObject.DestroyGameObject();
        }
    }
}