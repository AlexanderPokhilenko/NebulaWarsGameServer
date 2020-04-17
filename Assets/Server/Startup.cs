using System;
using System.Threading;
using System.Threading.Tasks;
using Server.GameEngine;
using Server.GameEngine.Experimental;
using Server.Http;
using Server.Udp;
using Server.Udp.Connection;
using Server.Udp.Sending;
using Server.Udp.Storage;

//TODO добавить di контейнер, когда сервер станет стабильным

namespace Server
{
    /// <summary>
    /// Запускает все потоки при старте и убивает их при остановке.
    /// Устанавливает зависимости.
    /// </summary>
    public class Startup
    {
        private const int HttpPort = 14065;
        private const int UdpListeningPort = 48956;
        
        private Thread httpListeningThread;
        private UdpListenerFacade udpListenerFacade;
        private Thread matchmakerNotifierThread;
        
        private MatchStorage matchStorage;
        private MatchRemover matchRemover;

        public void Run()
        {
            //Чек
            if (httpListeningThread != null)
            {
                throw new Exception("Сервер уже запущен");
            }

            //Старт уведомления матчмейкера о смертях игроков и окончании матчей
            MatchmakerMatchStatusNotifier matchStatusNotifier = new MatchmakerMatchStatusNotifier();
            matchmakerNotifierThread = matchStatusNotifier.StartThread();
            
            //Создание структур данных для матчей
            matchStorage = new MatchStorage();
            ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();
            UdpSendUtils udpSendUtils = new UdpSendUtils(matchStorage, byteArrayRudpStorage);
            matchRemover = new MatchRemover(matchStorage, byteArrayRudpStorage, udpSendUtils, matchStatusNotifier);
            MatchFactory matchFactory = new MatchFactory(matchRemover, udpSendUtils, matchStatusNotifier);
            MatchCreator matchCreator = new MatchCreator(matchFactory);
            MatchLifeCycleManager matchLifeCycleManager = 
                new MatchLifeCycleManager(matchStorage, matchCreator, matchRemover);
            InputEntitiesCreator inputEntitiesCreator = new InputEntitiesCreator(matchStorage);
            ExitEntitiesCreator exitEntitiesCreator = new ExitEntitiesCreator(matchStorage);
            GameEngineTicker gameEngineTicker = new GameEngineTicker(matchStorage, matchLifeCycleManager,
                inputEntitiesCreator, exitEntitiesCreator, byteArrayRudpStorage, udpSendUtils);

            //Старт прослушки
            httpListeningThread = StartMatchmakerListening(HttpPort, matchCreator, matchStorage);
            udpListenerFacade = StartPlayersListening(UdpListeningPort, inputEntitiesCreator, exitEntitiesCreator, 
                matchStorage, byteArrayRudpStorage, udpSendUtils);
            

            //Старт обработки
            Chronometer chronometer = ChronometerFactory.Create(gameEngineTicker.Tick);
            chronometer.StartEndlessLoop();
        }
        
        private Thread StartMatchmakerListening(int port, MatchCreator matchCreator, MatchStorage matchStorageArg)
        {
            MatchDataMessageHandler matchDataMessageHandler = 
                new MatchDataMessageHandler(matchCreator, matchStorageArg);
            Thread thread = new Thread(() =>
            {
                new HttpConnection(matchDataMessageHandler).StartListenHttp(port).Wait();
            });
            thread.Start();
            return thread;
        }

        private UdpListenerFacade StartPlayersListening(int port, InputEntitiesCreator inputEntitiesCreator, 
            ExitEntitiesCreator exitEntitiesCreator, MatchStorage matchStorageArg, 
            ByteArrayRudpStorage byteArrayRudpStorage, UdpSendUtils udpSendUtils)
        {

            UdpMediator mediator = new UdpMediator(inputEntitiesCreator, exitEntitiesCreator, matchStorageArg,
                byteArrayRudpStorage, udpSendUtils);
            
            var udpBattleConnectionLocal = new UdpListenerFacade(mediator);
            udpBattleConnectionLocal
                .SetUpConnection(port)
                .StartReceiveThread();
            
            return udpBattleConnectionLocal;
        }

        public void FinishAllMatches()
        {
            //TODO возможно lock поможет от одновременного вызова систем
            lock (matchRemover)
            {
                foreach (var match in matchStorage.GetAllMatches())
                {
                    matchRemover.MarkMatchAsFinished(match.MatchId);
                }
                matchRemover.DeleteFinishedMatches();    
            }
            //Жду, чтобы rudp о удалении матчей точно дошли до игроков
            Task.Delay(1500).Wait();
        }
        
        public void StopAllThreads()
        {
            httpListeningThread.Interrupt();
            udpListenerFacade.Stop();
            matchmakerNotifierThread.Interrupt();
        }
    }
}