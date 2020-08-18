using System.Collections.Generic;
using UnityEngine;

namespace SharedSimulationCode.LagCompensation
{
    public class GameState
    {
        public readonly int tickNumber;
        public readonly Dictionary<ushort, Transform> transforms;
        
        public GameState(int tickNumber, Dictionary<ushort, Transform> transforms)
        {
            this.tickNumber = tickNumber;
            this.transforms = transforms;
        }
    }
}