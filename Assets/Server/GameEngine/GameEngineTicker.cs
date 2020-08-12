using Server.GameEngine.Experimental;
using Server.GameEngine.MatchLifecycle;
using Server.GameEngine.MessageSorters;
using Server.GameEngine.Rudp;
using SharedSimulationCode;

namespace Server.GameEngine
{
    /// <summary>
    /// Отвечает за правильный вызов подпрограмм во время тика.
    /// </summary>
    public class GameEngineTicker
    {
        private readonly MatchStorage matchStorage;
        private readonly RudpMessagesSender rudpMessagesSender;
        private readonly ExitEntitiesCreator exitEntitiesCreator;
        private readonly InputEntitiesCreator inputEntitiesCreator;
        private readonly MatchLifeCycleManager matchLifeCycleManager;
        private readonly OutgoingMessagesStorage outgoingMessagesStorage;

        public GameEngineTicker(MatchStorage matchStorage, MatchLifeCycleManager matchLifeCycleManager,
            InputEntitiesCreator inputEntitiesCreator, ExitEntitiesCreator exitEntitiesCreator,
            RudpMessagesSender rudpMessagesSender, OutgoingMessagesStorage outgoingMessagesStorage)
        {
            this.matchStorage = matchStorage;
            this.rudpMessagesSender = rudpMessagesSender;
            this.exitEntitiesCreator = exitEntitiesCreator;
            this.inputEntitiesCreator = inputEntitiesCreator;
            this.matchLifeCycleManager = matchLifeCycleManager;
            this.outgoingMessagesStorage = outgoingMessagesStorage;
        }

        public void Tick()
        {
            inputEntitiesCreator.Create();
            exitEntitiesCreator.Create();
            
            //Перемещение игровых сущностей и создание сообщений с новым состоянием игрового мира
            foreach (MatchSimulation match in matchStorage.GetAllMatches())
            {
                match.Tick();
            }

            //добавление rudp к списку того, что нужно отправить
            rudpMessagesSender.SendAll();
            
            //Отправка созданных сообщений
            outgoingMessagesStorage.SendAllMessages();
            
            //создание/удаление матчей
            matchLifeCycleManager.UpdateMatchesLifeStatus();
        }
    }
}