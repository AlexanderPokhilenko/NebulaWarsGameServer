using Server.GameEngine.Experimental;
using UnityEngine;

namespace Server.GameEngine
{
    public class GameEngineMediator
    {
        private Clock clock;
        private readonly IRudpSender rudpSender;
        public static BattlesStorage BattlesStorage;

        public GameEngineMediator()
        {
            BattlesStorage = new BattlesStorage();
            rudpSender = new SimpleRudpSender(BattlesStorage);
            InitClock();
        }

        private void InitClock()
        {
            #if UNITY_5_3_OR_NEWER
                var go = new GameObject("Clock");
                clock = go.AddComponent<Clock>();
                clock.gameEngineMediator = this;
            #else
                clock = new Clock(this);
            #endif
        }
        
        public void StartEndlessLoop()
        {
            clock.StartEndlessLoop();
        }
        
        public void Tick()
        {
            StaticInputMessagesSorter.Spread();
            foreach (var gameSession in BattlesStorage.GetAllGameSessions())
            {
                gameSession.Execute();
                gameSession.Cleanup();
            }
            PingLogger.Log();
            rudpSender.SendUnconfirmedMessages();
            BattlesStorage.UpdateGameSessionsState();
        }
    }
}