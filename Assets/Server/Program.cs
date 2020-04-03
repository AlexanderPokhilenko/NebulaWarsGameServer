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
            StartGameMatcherListenerThread(HttpPort);
            StartPlayersListenerThread(UdpPort);
            
            MatchDeletingNotifier.StartThread();
            PlayerDeathNotifier.StartThread();
            
            GameEngineMediator gameEngineMediator = new GameEngineMediator();
            gameEngineMediator.StartEndlessLoop();
        }
        

        private static void StartGameMatcherListenerThread(int port)
        {
            new Thread(() => { new HttpListenerWrapper().StartListenHttp(port).Wait(); })
                .Start();
        }

        private static void StartPlayersListenerThread(int port)
        {
            NetworkMediator mediator = new NetworkMediator();
            var udpBattleConnection = new UdpBattleConnection(mediator);
            udpBattleConnection
                .SetUpConnection(port)
                .StartReceiveThread();
        }
    }
}