using System;
using System.Collections;

namespace SharedSimulationCode.LagCompensation
{
    public class LagCompensationSystem
    {
        public GameState PastState { get; set; }
        public GameState PresentState { get; set; }

        public void Execute(object entity)
        {
            throw new NotImplementedException();
        }
    }
}