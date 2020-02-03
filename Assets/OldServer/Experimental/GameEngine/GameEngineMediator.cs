
using UnityEngine;

namespace AmoebaBattleServer01.Experimental.GameEngine
{
    public class GameEngineMediator
    {
        public static readonly GameSessionsStorage GameSessionsStorage = new GameSessionsStorage();
        private readonly Clock clock;

        public GameEngineMediator()
        {
#if UNITY_5_3_OR_NEWER
            var go = new GameObject("Clock");
            clock = go.AddComponent<Clock>();
            clock.gameEngineMediator = this;
#else
            clock = new Clock(this);
#endif
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