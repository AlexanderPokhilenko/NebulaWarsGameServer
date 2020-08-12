using UnityEngine;
using UnityEngine.SceneManagement;

namespace SharedSimulationCode.PhysicsSceneTesting
{
    public class PhysicsSceneTest : MonoBehaviour
    {
        private PhysicsScene physicsScene;

        private void Start()
        {
            var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
            Scene additiveScene = SceneManager.LoadScene("AdditiveScene", loadSceneParameters);
            physicsScene = additiveScene.GetPhysicsScene();
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            SceneManager.MoveGameObjectToScene(cylinder, additiveScene);
        }

        private void FixedUpdate()
        {
            if (!physicsScene.IsValid())
            {
                physicsScene.Simulate(Time.fixedDeltaTime);
            }
        }
    }
}