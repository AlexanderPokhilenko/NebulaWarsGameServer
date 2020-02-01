
namespace AmoebaBattleServer01.Experimental.GameEngine
{
    public class GameEngineMediator
    {
        public static readonly GameSessionsStorage GameSessionsStorage = new GameSessionsStorage();
        private readonly Clock clock;

        public GameEngineMediator()
        {
            clock = new Clock(this);
        }
        
        public void Tick()
        {
            StaticInputMessagesSorter.Spread();
            foreach (var gameSession in GameSessionsStorage.GetAllGameSessions())
            {
                gameSession.Execute();
                gameSession.Cleanup();
            }
            PingLogger.Log();
            GameSessionsStorage.UpdateGameSessions();
        }

        public void StartEndlessLoop()
        {
            clock.StartEndlessLoop();
        }
    }
}