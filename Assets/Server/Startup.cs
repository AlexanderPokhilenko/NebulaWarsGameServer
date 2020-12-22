using System;
using System.Threading;
using System.Threading.Tasks;
using Server.GameEngine;
using Server.GameEngine.Chronometers;
using Server.GameEngine.MatchLifecycle;
using Server.GameEngine.MessageSorters;
using Server.GameEngine.Rudp;
using Server.Http;
using Server.Udp.Connection;
using Server.Udp.MessageProcessing;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server
{
    /// <summary>
    /// Запускает все потоки при старте и убивает их при остановке.
    /// Устанавливает зависимости.
    /// </summary>
    public class Startup
    {
        private MatchStorage matchStorage;
        private MatchRemover matchRemover;
        private const int UdpPort = 48956;
        private const int HttpPort = 14065;
        private ShittyUdpMediator shittyUdpMediator;
        private CancellationTokenSource matchmakerNotifierCts;
        private CancellationTokenSource matchmakerListenerCts;
        private CancellationTokenSource monitorListenerCts;

        public void Run()
        {
            if (matchmakerListenerCts != null)
            {
                throw new Exception("Сервер уже запущен");
            }

            //Старт уведомления матчмейкера о смертях игроков и окончании матчей
            MatchmakerNotifier notifier = new MatchmakerNotifier();
            matchmakerNotifierCts = notifier.StartThread();
            
            //Создание структур данных для матчей
            matchStorage = new MatchStorage();


            MessageIdFactory messageIdFactory = new MessageIdFactory();
            MessageFactory messageFactory = new MessageFactory(messageIdFactory);
            
            InputEntitiesCreator inputEntitiesCreator = new InputEntitiesCreator(matchStorage);
            ExitEntitiesCreator exitEntitiesCreator = new ExitEntitiesCreator(matchStorage);
            
            ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();

            shittyUdpMediator = new ShittyUdpMediator();
            
            MessagesPackIdFactory messagesPackIdFactory = new MessagesPackIdFactory();
            IpAddressesStorage ipAddressesStorage = new IpAddressesStorage();
            SimpleMessagesPacker simpleMessagesPacker = new SimpleMessagesPacker(PackingHelper.Mtu, shittyUdpMediator, messagesPackIdFactory);
            OutgoingMessagesStorage outgoingMessagesStorage = new OutgoingMessagesStorage(simpleMessagesPacker, ipAddressesStorage);
            UdpSendUtils udpSendUtils = new UdpSendUtils(byteArrayRudpStorage, outgoingMessagesStorage, messageFactory);
            MessageProcessor messageProcessor = new MessageProcessor(inputEntitiesCreator, exitEntitiesCreator, 
                byteArrayRudpStorage, 
                // udpSendUtils,
                ipAddressesStorage);
            
            shittyUdpMediator.SetProcessor(messageProcessor);
            
            matchRemover = new MatchRemover(matchStorage, byteArrayRudpStorage, udpSendUtils, notifier, ipAddressesStorage, messageIdFactory, messagesPackIdFactory);
            MatchFactory matchFactory = new MatchFactory(matchRemover, udpSendUtils, notifier, ipAddressesStorage, messageIdFactory, messagesPackIdFactory);
            MatchCreator matchCreator = new MatchCreator(matchFactory);
            MatchLifeCycleManager matchLifeCycleManager = 
                new MatchLifeCycleManager(matchStorage, matchCreator, matchRemover);
            
            //Старт прослушки матчмейкера
            MatchModelMessageHandler matchModelMessageHandler = new MatchModelMessageHandler(matchCreator, matchStorage);
            MatchmakerListener matchmakerListener = new MatchmakerListener(matchModelMessageHandler, HttpPort);
            MonitorListener monitorListener = new MonitorListener(HttpPort);
            monitorListenerCts = monitorListener.StartThread();
            matchmakerListenerCts = matchmakerListener.StartThread();


            //Старт прослушки игроков
            shittyUdpMediator
                .SetupConnection(UdpPort)
                .StartReceiveThread();

            RudpMessagesSender rudpMessagesSender = new RudpMessagesSender(byteArrayRudpStorage, matchStorage, udpSendUtils, ipAddressesStorage);
            GameEngineTicker gameEngineTicker = new GameEngineTicker(matchStorage, matchLifeCycleManager,
                inputEntitiesCreator, exitEntitiesCreator, rudpMessagesSender, outgoingMessagesStorage);
            
            //Старт тиков
            Chronometer chronometer = ChronometerFactory.Create(gameEngineTicker.Tick);
            chronometer.StartEndlessLoop();
        }
        
        public void FinishAllMatches()
        {
            //TODO возможно lock поможет от одновременного вызова систем
            lock (matchRemover)
            {
                foreach (var match in matchStorage.GetAllMatches())
                {
                    matchRemover.MarkMatchAsFinished(match.matchId);
                }
                matchRemover.DeleteFinishedMatches();    
            }
            //Жду, чтобы rudp о удалении матчей точно дошли до игроков
            Task.Delay(1500).Wait();
        }
        
        public void StopAllThreads()
        {
            shittyUdpMediator.Stop();
            matchmakerNotifierCts.Cancel();
            matchmakerListenerCts.Cancel();
            monitorListenerCts.Cancel();
        }
    }
}