using Entitas;
using Server.GameEngine.Chronometers;
using UnityEngine;

namespace SharedSimulationCode
{
    public class ChronomenterDeltaTimeDebugSystem : IExecuteSystem
    { 
        public void Execute()
        {
            float current = Chronometer.DeltaTime;
            Debug.LogError(current);
        }
    }
}