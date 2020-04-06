using System;
using Server.GameEngine.Experimental;
using UnityEngine;

namespace Server.GameEngine
{
    /// <summary>
    /// Вызывает метод раз в период.
    /// </summary>
    public class ClockFacade
    {
        private readonly Clock clock;

        public ClockFacade(Action action)
        {
#if UNITY_5_3_OR_NEWER
            var go = new GameObject("Clock");
            clock = go.AddComponent<Clock>();
            clock.SetAction(action);
#else
            clock = new Clock(this);
#endif
        }
   
        public void StartEndlessLoop()
        {
            clock.StartEndlessLoop();
        }
    }
    
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class GameEngineMediator
    {
        private readonly IRudpSender rudpSender;
        private readonly ClockFacade clockFacade;
        public static MatchStorageFacade MatchStorageFacade;

        public GameEngineMediator()
        {
            MatchStorageFacade = new MatchStorageFacade();
            rudpSender = new SimpleRudpSender(MatchStorageFacade);
            clockFacade = new ClockFacade(Tick);
        }

        public void StartEndlessLoop()
        {
            clockFacade.StartEndlessLoop();
        }

        private void Tick()
        {
            StaticInputMessagesSorter.Spread();

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
            foreach (var gameSession in MatchStorageFacade.GetAllGameSessions())
            {
                gameSession.Execute();
                gameSession.Cleanup();
            }
#else
            Parallel.ForEach(MatchStorageFacade.GetAllGameSessions(), gameSession =>
            {
                gameSession.Execute();
                gameSession.Cleanup();
            });
#endif

            PingLogger.Log();
            rudpSender.SendUnconfirmedMessages();
            MatchStorageFacade.UpdateBattlesList();
        }
    }
}