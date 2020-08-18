using Entitas;
using Server.GameEngine.Chronometers;
using UnityEngine;

namespace SharedSimulationCode.Systems.Statistics
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