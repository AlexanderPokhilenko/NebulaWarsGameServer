using Entitas;
using Server.GameEngine.Chronometers;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Вызывает обработку физики
    /// </summary>
    public class PhysicsSimulateSystem : IExecuteSystem
    {
        private readonly PhysicsScene physicsScene;

        public PhysicsSimulateSystem(PhysicsScene physicsScene)
        {
            this.physicsScene = physicsScene;
        }

        public void Execute()
        {
            physicsScene.Simulate(Chronometer.DeltaTime);
        }
    }
}