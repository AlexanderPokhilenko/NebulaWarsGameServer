using System.Collections.Generic;
using System.Net;
using Server.GameEngine.Experimental;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    public class OutgoingMessagesStorage
    {
        private readonly ShittyDatagramPacker shittyDatagramPacker;

        private readonly Dictionary<IPEndPoint, List<byte[]>> messages = new Dictionary<IPEndPoint, List<byte[]>>();
        
        public OutgoingMessagesStorage(ShittyDatagramPacker shittyDatagramPacker)
        {
            this.shittyDatagramPacker = shittyDatagramPacker;
        }
        
        public void AddMessage(byte[] data, IPEndPoint ipEndPoint)
        {
            if (messages.TryGetValue(ipEndPoint, out var playerMessages))
            {
                playerMessages.Add(data);
            }
            else
            {
                //TODO сколько сообщений отправляется игроку за кадр в среднем?
                messages.Add(ipEndPoint, new List<byte[]>(5){data} );
            }
        }
        
        public void SendAllMessages()
        {
            foreach (var pair in messages)
            {
                shittyDatagramPacker.Send(pair.Key, pair.Value);
            }
            
            messages.Clear();
        }
    }
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