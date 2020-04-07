using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
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
            StaticExitMessageSorter.Spread();

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