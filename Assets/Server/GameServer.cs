using System.Threading;
using Server.GameEngine;
using Server.Http;
using Server.Udp;
using Server.Udp.Connection;

namespace Server
{
    public class GameServer
    {
        private const int HttpPort = 14065;
        private const int UdpPort = 48956;
        
        private Thread httpListeningThread;
        private UdpConnectionFacade udpConnectionFacade;
        private Thread matchDeletingNotifierThread;
        private Thread playerDeathNotifierThread;
        
        public void Run()
        {
            httpListeningThread = StartMatchmakerListening(HttpPort);
            udpConnectionFacade = StartPlayersListening(UdpPort);

            matchDeletingNotifierThread = MatchDeletingNotifier.StartThread();
            playerDeathNotifierThread = PlayerDeathNotifier.StartThread();

            MatchManager matchManager = new MatchManager();
            Chronometer chronometer = ChronometerFactory.Create(matchManager.Tick);
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

        public void StopListeningThreads()
        {
            httpListeningThread.Interrupt();
            udpConnectionFacade.Stop();
            matchDeletingNotifierThread.Interrupt();
            playerDeathNotifierThread.Interrupt();
        }
    }
}