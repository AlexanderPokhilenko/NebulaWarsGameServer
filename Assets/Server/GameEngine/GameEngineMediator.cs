using Server.GameEngine.Experimental;
using UnityEngine;

namespace Server.GameEngine
{
    public class GameEngineMediator
    {
        private Clock clock;
        private readonly IRudpSender rudpSender;
        public static MatchStorage MatchStorage;

        public GameEngineMediator()
        {
            MatchStorage = new MatchStorage();
            rudpSender = new SimpleRudpSender(MatchStorage);
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

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
            foreach (var gameSession in MatchStorage.GetAllGameSessions())
            {
                gameSession.Execute();
                gameSession.Cleanup();
            }
#else
            Parallel.ForEach(MatchStorage.GetAllGameSessions(), gameSession =>
            {
                gameSession.Execute();
                gameSession.Cleanup();
            });
#endif

            PingLogger.Log();
            rudpSender.SendUnconfirmedMessages();
            MatchStorage.UpdateBattlesList();
        }
    }
}