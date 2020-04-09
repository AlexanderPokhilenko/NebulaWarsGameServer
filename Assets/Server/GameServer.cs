using System;
using System.Threading;
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
    public class GameServer
    {
        private const int HttpPort = 14065;
        private const int UdpPort = 48956;
        
        private Thread httpListeningThread;
        private UdpConnectionFacade udpConnectionFacade;
        private Thread matchDeletingNotifierThread;
        private Thread playerDeathNotifierThread;
        
        private MatchStorage matchStorage;

        public void Run()
        {
            //Чек
            if (httpListeningThread != null)
            {
                throw new Exception("Сервер уже запущен");
            }


            //Создание структур данных для матчей
            matchStorage = new MatchStorage();
            ByteArrayRudpStorage byteArrayRudpStorage = new ByteArrayRudpStorage();
            UdpSendUtils.Initialize(matchStorage, byteArrayRudpStorage);
            
            MatchRemover matchRemover = new MatchRemover(matchStorage);
            MatchFactory matchFactory = new MatchFactory(matchRemover);
            MatchCreator matchCreator = new MatchCreator(matchFactory);
            MatchLifeCycleManager matchLifeCycleManager = 
                new MatchLifeCycleManager(matchStorage, matchCreator, matchRemover);
            InputEntitiesCreator inputEntitiesCreator = new InputEntitiesCreator(matchStorage);
            ExitEntitiesCreator exitEntitiesCreator = new ExitEntitiesCreator(matchStorage);

            GameEngineTicker gameEngineTicker = new GameEngineTicker(matchStorage, matchLifeCycleManager,
                inputEntitiesCreator, exitEntitiesCreator, byteArrayRudpStorage);

            Chronometer chronometer = ChronometerFactory.Create(gameEngineTicker.Tick);
            
            





            //Старт прослушки
            httpListeningThread = StartMatchmakerListening(HttpPort, matchCreator, matchStorage);
            udpConnectionFacade = StartPlayersListening(UdpPort, inputEntitiesCreator, exitEntitiesCreator, 
                matchStorage, byteArrayRudpStorage);
            matchDeletingNotifierThread = MatchDeletingNotifier.StartThread();
            playerDeathNotifierThread = PlayerDeathNotifier.StartThread();
           
            
            
            
            
            
            
            
            //Старт обработки
            chronometer.StartEndlessLoop();
        }
        
        private Thread StartMatchmakerListening(int port, MatchCreator matchCreator, MatchStorage matchStorageArg)
        {
            MatchDataMessageHandler matchDataMessageHandler = new MatchDataMessageHandler(matchCreator, matchStorageArg);
            Thread thread = new Thread(() => { new HttpConnection(matchDataMessageHandler).StartListenHttp(port).Wait(); });
            thread.Start();
            return thread;
        }

        private UdpConnectionFacade StartPlayersListening(int port, InputEntitiesCreator inputEntitiesCreator, 
            ExitEntitiesCreator exitEntitiesCreator, MatchStorage matchStorageArg, ByteArrayRudpStorage byteArrayRudpStorage)
        {
            var udpBattleConnectionLocal = new UdpConnectionFacade();

            UdpMediator mediator = new UdpMediator(inputEntitiesCreator, exitEntitiesCreator, matchStorageArg,
                byteArrayRudpStorage);
            mediator.SetUdpConnection(udpBattleConnectionLocal);
            
            udpBattleConnectionLocal
                .SetUpConnection(port)
                .StartReceiveThread();
            
            return udpBattleConnectionLocal;
        }

        public void FinishAllMatches()
        {
            throw new NotImplementedException();
        }
        
        public void StopListeningThreads()
        {
            httpListeningThread.Interrupt();
            udpConnectionFacade.Stop();
            matchDeletingNotifierThread.Interrupt();
            playerDeathNotifierThread.Interrupt();
        }
    }
}