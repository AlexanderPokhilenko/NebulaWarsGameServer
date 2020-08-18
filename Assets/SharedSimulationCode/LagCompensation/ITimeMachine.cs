namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Откатывает все физические обьекты на определённый момент времени 
    /// </summary>
    public interface ITimeMachine
    {
        GameState TravelToTime(int tickNumber);
        void SetActualGameState(GameState gameState);
    }
}