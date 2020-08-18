using System;
using System.Collections;

namespace SharedSimulationCode.LagCompensation
{
    public abstract class LagCompensationSystem
    {
        // public abstract GameState PastState { get; set; }
        // public abstract GameState PresentState { get; set; }

        public abstract void Execute(GameEntity entity);
    }
}