namespace SharedSimulationCode.LagCompensation
{
    public interface IGameStateHistory
    {
        GameState Get(int tick);
    }
}