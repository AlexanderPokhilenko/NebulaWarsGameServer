using System;
using System.Threading;
using Server.GameEngine;
using Server.Http;
using Server.Udp;
using Server.Udp.Connection;

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
            if (httpListeningThread != null)
            {
                throw new Exception("Сервер уже запущен");
            }
            
            httpListeningThread = StartMatchmakerListening(HttpPort);
            udpConnectionFacade = StartPlayersListening(UdpPort);

            matchDeletingNotifierThread = MatchDeletingNotifier.StartThread();
            playerDeathNotifierThread = PlayerDeathNotifier.StartThread();

            
            matchStorage = new MatchStorage();
            MatchLifeCycleManager matchLifeCycleManager = new MatchLifeCycleManager();
            
            GameEngineTicker gameEngineTicker = new GameEngineTicker(matchStorage, matchLifeCycleManager);
            Chronometer chronometer = ChronometerFactory.Create(gameEngineTicker.Tick);
            chronometer.StartEndlessLoop();
        }
        
        private Thread StartMatchmakerListening(int port)
        {
            Thread thread = new Thread(() => { new HttpListenerWrapper().StartListenHttp(port).Wait(); });
            thread.Start();
            return thread;
        }

        private UdpConnectionFacade StartPlayersListening(int port)
        {
            var udpBattleConnectionLocal = new UdpConnectionFacade();

            NetworkMediator mediator = new NetworkMediator();
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