using System;
using System.Threading;
using System.Threading.Tasks;
using Plugins.submodules.SharedCode;
using Plugins.submodules.SharedCode.Systems.Spawn;
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
using Object = UnityEngine.Object;

namespace Server
{
    /// <summary>
    /// Запускает все потоки при старте и убивает их при остановке.
    /// Устанавливает зависимости.
    /// </summary>
    public class Startup
    {
        private MatchRemover matchRemover;
        private MatchesStorage matchesStorage;
        private const int UdpPort = 48956;
        private const int HttpPort = 14065;
        private CancellationTokenSource matchmakerNotifierCts;
        private CancellationTokenSource matchmakerListenerCts;
        private UdpClientWrapper udpClientWrapper;

        public void Run(WarshipsCharacteristicsStorage warshipsCharacteristicsStorage)
        {
            if (matchmakerListenerCts != null)
            {
                throw new Exception("Сервер уже запущен");
            }

            PrefabsStorage prefabsStorage = new PrefabsStorage();
            Chronometer chronometer = Object.FindObjectOfType<Chronometer>();
            

            //Старт уведомления матчмейкера о смертях игроков и окончании матчей
            MatchmakerNotifier notifier = new MatchmakerNotifier();
            matchmakerNotifierCts = notifier.StartThread();
            
            //Создание структур данных для матчей
            matchesStorage = new MatchesStorage();


            MessageIdFactory messageIdFactory = new MessageIdFactory();
            MessageFactory messageFactory = new MessageFactory(messageIdFactory);
            
            InputEntitiesCreator inputEntitiesCreator = new InputEntitiesCreator(matchesStorage);
            ExitEntitiesCreator exitEntitiesCreator = new ExitEntitiesCreator(matchesStorage);
            
            ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();

            
            
            udpClientWrapper = new UdpClientWrapper();
            MessagesPackIdFactory messagesPackIdFactory = new MessagesPackIdFactory();
            IpAddressesStorage ipAddressesStorage = new IpAddressesStorage();
            SimpleMessagesPacker simpleMessagesPacker = new SimpleMessagesPacker(PackingHelper.Mtu, udpClientWrapper, messagesPackIdFactory);
            OutgoingMessagesStorage outgoingMessagesStorage = new OutgoingMessagesStorage(simpleMessagesPacker, ipAddressesStorage);
            UdpSendUtils udpSendUtils = new UdpSendUtils(ipAddressesStorage, byteArrayRudpStorage, outgoingMessagesStorage, messageFactory);
            MessageWrapperHandler messageWrapperHandler = new MessageWrapperHandler(
                inputEntitiesCreator,
                exitEntitiesCreator, 
                byteArrayRudpStorage,
                udpClientWrapper,
                ipAddressesStorage);
            
            IByteArrayHandler byteArrayHandler = new ByteArrayHandler(messageWrapperHandler);

#if UNITY_EDITOR
            byteArrayHandler = new JitterSimulation(byteArrayHandler, udpClientWrapper, 50, 100); 
#endif
            
            matchRemover = new MatchRemover(matchesStorage, byteArrayRudpStorage, udpSendUtils, notifier, 
                ipAddressesStorage, messageIdFactory, messagesPackIdFactory);
            MatchFactory matchFactory = new MatchFactory(matchRemover, udpSendUtils, notifier, ipAddressesStorage,
                messageIdFactory, messagesPackIdFactory, chronometer, chronometer, prefabsStorage, warshipsCharacteristicsStorage);
            MatchCreator matchCreator = new MatchCreator(matchFactory);
            MatchLifeCycleManager matchLifeCycleManager = 
                new MatchLifeCycleManager(matchesStorage, matchCreator, matchRemover);
            
            //Старт прослушки матчмейкера
            MatchModelMessageHandler matchModelMessageHandler = new MatchModelMessageHandler(matchCreator, matchesStorage);
            MatchmakerListener matchmakerListener = new MatchmakerListener(matchModelMessageHandler, HttpPort);
            matchmakerListenerCts = matchmakerListener.StartThread();
            
            //Старт прослушки игроков
            udpClientWrapper
                .SetupConnection(UdpPort)
                .StartReceiveThread(byteArrayHandler);

            RudpMessagesSender rudpMessagesSender = new RudpMessagesSender(byteArrayRudpStorage, matchesStorage, udpSendUtils, ipAddressesStorage);
            GameEngineTicker gameEngineTicker = new GameEngineTicker(matchesStorage, matchLifeCycleManager,
                inputEntitiesCreator, exitEntitiesCreator, rudpMessagesSender, outgoingMessagesStorage);
            
            //Старт тиков
            
            chronometer.SetCallback(gameEngineTicker.Tick);
            chronometer.StartEndlessLoop();
        }
        
        public void FinishAllMatches()
        {
            //TODO возможно lock поможет от одновременного вызова систем
            lock (matchRemover)
            {
                foreach (var match in matchesStorage.GetAllMatches())
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
            udpClientWrapper.Stop();
            matchmakerNotifierCts.Cancel();
            matchmakerListenerCts.Cancel();
        }
    }
}