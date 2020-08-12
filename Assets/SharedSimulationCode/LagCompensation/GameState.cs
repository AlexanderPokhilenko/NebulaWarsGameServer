namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Хранит transform всех обьектов для тика
    /// </summary>
    public class GameState
    {
        public readonly int time;

        public GameState(int time)
        {
            this.time = time;
        }
    }
}