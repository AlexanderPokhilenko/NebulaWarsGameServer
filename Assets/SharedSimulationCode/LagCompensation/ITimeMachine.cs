namespace SharedSimulationCode.LagCompensation
{
    public interface ITimeMachine
    {
        GameState TravelToTime(int tick);
    }
}