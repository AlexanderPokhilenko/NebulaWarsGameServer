using System.Threading;
using Server.GameEngine;
using Server.Http;
using Server.Udp;
using Server.Udp.Connection;

namespace Server
{
    static class Program
    {
        private const int HttpPort = 14065;
        private const int UdpPort = 48956;
        
        public static void Main()
        {
            StartMatchmakerListening(HttpPort);
            StartPlayersListening(UdpPort);
            
            MatchDeletingNotifier.StartThread();
            PlayerDeathNotifier.StartThread();
            
            GameEngineMediator gameEngineMediator = new GameEngineMediator();
            gameEngineMediator.StartEndlessLoop();
        }
        

        private static void StartMatchmakerListening(int port)
        {
            new Thread(() => { new HttpListenerWrapper().StartListenHttp(port).Wait(); })
                .Start();
        }

        private static void StartPlayersListening(int port)
        {
            var udpBattleConnection = new UdpBattleConnection();

            NetworkMediator mediator = new NetworkMediator();
            mediator.SetUdpConnection(udpBattleConnection);
            
            udpBattleConnection
                .SetUpConnection(port)
                .StartReceiveThread();
        }
    }
}