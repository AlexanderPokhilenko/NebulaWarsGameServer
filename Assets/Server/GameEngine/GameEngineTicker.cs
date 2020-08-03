using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class GameEngineTicker
    {
        private readonly MatchStorage matchStorage;
        private readonly MatchLifeCycleManager matchLifeCycleManager;
        private readonly InputEntitiesCreator inputEntitiesCreator;
        private readonly ExitEntitiesCreator exitEntitiesCreator;
        private readonly RudpMessagesSender rudpMessagesSender;
        private readonly OutgoingMessagesStorage outgoingMessagesStorage;

        public GameEngineTicker(MatchStorage matchStorage, MatchLifeCycleManager matchLifeCycleManager,
            InputEntitiesCreator inputEntitiesCreator, ExitEntitiesCreator exitEntitiesCreator,
            RudpMessagesSender rudpMessagesSender, OutgoingMessagesStorage outgoingMessagesStorage)
        {
            this.matchStorage = matchStorage;
            this.matchLifeCycleManager = matchLifeCycleManager;
            this.inputEntitiesCreator = inputEntitiesCreator;
            this.exitEntitiesCreator = exitEntitiesCreator;
            this.rudpMessagesSender = rudpMessagesSender;
            this.outgoingMessagesStorage = outgoingMessagesStorage;
        }

        public void Tick()
        {
            inputEntitiesCreator.Create();
            exitEntitiesCreator.Create();
            
            //Перемещение игровых сущностей и создание сообщений с новым состоянием игрового мира
            foreach (Match match in matchStorage.GetAllMatches())
            {
                match.Tick();
            }

            //добавление rudp к списку того, что нужно отправить
            rudpMessagesSender.SendAll();
            
            //Отправка созданных сообщений игрокам
            outgoingMessagesStorage.SendAllMessages();
            
            //создание/удаление матчей
            matchLifeCycleManager.UpdateMatchesLifeStatus();
        }
    }
}