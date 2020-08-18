namespace SharedSimulationCode.LagCompensation
{
    public interface IGameStateHistory
    {
        GameState Get(int tickNumber);
        int GetTickNumber();
        void Add(GameState gameState);
        GameState GetActualGameState();
    }
}