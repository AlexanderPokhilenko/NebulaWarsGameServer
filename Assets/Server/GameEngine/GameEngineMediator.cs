using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class GameEngineMediator
    {
        private readonly ClockFacade clockFacade;
        public static MatchStorageFacade MatchStorageFacade;
        private readonly IRudpSender rudpSender;

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
            //Создание сущностей после ввода игроков
            StaticInputMessagesSorter.Spread();
            StaticExitMessageSorter.Spread();
            
            //Обработка 
            foreach (var gameSession in MatchStorageFacade.GetAllGameSessions())
            {
                gameSession.Execute();
                gameSession.Cleanup();
            }

            //Отправка rudp
            rudpSender.SendUnconfirmedMessages();
            //создание/удаление матчей
            MatchStorageFacade.UpdateBattlesList();
        }
    }
}