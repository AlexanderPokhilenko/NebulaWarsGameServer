namespace SharedSimulationCode.LagCompensation
{
    public interface IGameStateHistory
    {
        GameState Get(int tickNumber);
        int GetLastTickNumber();
        void Add(GameState gameState);
        GameState GetActualGameState();
    }
}