using System.Threading;
using log4net.Util;
using Server.GameEngine;
using Server.Http;
using Server.Udp;
using Server.Udp.Connection;

namespace Server
{
    class GameServer
    {
        private const int HttpPort = 14065;
        private const int UdpPort = 48956;
        
        private Thread httpListeningThread;
        private UdpBattleConnection udpBattleConnection;
        private Thread matchDeletingNotifier;
        private Thread playerDeathNotifierThread;
        
        public void Run()
        {
            httpListeningThread = StartMatchmakerListening(HttpPort);
            udpBattleConnection = StartPlayersListening(UdpPort);

            matchDeletingNotifier = MatchDeletingNotifier.StartThread();
            playerDeathNotifierThread = PlayerDeathNotifier.StartThread();

            GameEngineMediator gameEngineMediator = new GameEngineMediator();
            gameEngineMediator.StartEndlessLoop();
        }
        
        private Thread StartMatchmakerListening(int port)
        {
            Thread thread = new Thread(() => { new HttpListenerWrapper().StartListenHttp(port).Wait(); });
            thread.Start();
            return thread;
        }

        private UdpBattleConnection StartPlayersListening(int port)
        {
            udpBattleConnection = new UdpBattleConnection();

            NetworkMediator mediator = new NetworkMediator();
            mediator.SetUdpConnection(udpBattleConnection);
            
            udpBattleConnection
                .SetUpConnection(port)
                .StartReceiveThread();
            
            return udpBattleConnection;
        }

        public void StopListeningThreads()
        {
            httpListeningThread.Interrupt();
            udpBattleConnection.Stop();
            matchDeletingNotifier.Interrupt();
            playerDeathNotifierThread.Interrupt();
        }
    }
}