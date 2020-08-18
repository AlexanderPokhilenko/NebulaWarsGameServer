using System;

namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Расставляет GameObject-ы по позициям согласно GameState
    /// </summary>
    public class TimeMachine : ITimeMachine
    {
        /// <summary>
        /// Текущее игровое состояние на сервере
        /// </summary>
        private GameState actualState;
        /// <summary>
        /// История игровых состояний
        /// </summary>
        private readonly IGameStateHistory history;
        /// <summary>
        /// Набор систем, расставляющих коллайдеры в физическом мире по данным из игрового состояния
        /// </summary>
        private readonly ArrangeTransformSystem[] arrangeTransformSystems;

        public TimeMachine(IGameStateHistory history, ArrangeTransformSystem[] timeInitArrangeTransformSystems)
        {
            this.history = history; 
            arrangeTransformSystems = timeInitArrangeTransformSystems;  
        }

        public void SetActualGameState(GameState gameState)
        {
            actualState = gameState;
        }

        public GameState TravelToTime(int tickNumber)
        {
            GameState pastState;
            if (tickNumber == actualState.tickNumber)
            {
                pastState = actualState;
            }
            else
            {
                pastState = history.Get(tickNumber);
            }
            
            foreach (var system in arrangeTransformSystems)
            {
                system.GameState = pastState;
                system.Execute();
            }
            
            return pastState;
        }
    }
}