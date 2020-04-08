using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    //TODO название не очень
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class MatchManager
    {
        public static MatchStorageFacade MatchStorageFacade;
        private readonly IRudpSender rudpSender;

        public MatchManager()
        {
            MatchStorageFacade = new MatchStorageFacade();
            rudpSender = new SimpleRudpSender(MatchStorageFacade);
        }

        public void Tick()
        {
            //Создание сущностей ввода игроков
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