using System.Threading;
using Server.GameEngine;
using Server.Http;
using Server.Udp;
using Server.Udp.Connection;

namespace Server
{
    static class Program
    {
        public static void Main()
        {
            GameEngineMediator gameEngineMediator = new GameEngineMediator();
            
            StartListenGameMatcherInAnotherThread(14065);
            StartListenPlayersInAnotherThread(48956);
            StartGameRoomDeletingNotifierThread();
            
            gameEngineMediator.StartEndlessLoop();
        }
        

        private static void StartListenGameMatcherInAnotherThread(int port)
        {
            HttpListenerWrapper httpListenerWrapper = new HttpListenerWrapper();
            new Thread(() => { httpListenerWrapper.StartListenHttp(port).Wait(); }).Start();
        }

        private static void StartListenPlayersInAnotherThread(int port)
        {
            NetworkMediator mediator = new NetworkMediator();
            var udpBattleConnection = new UdpBattleConnection(mediator);
            udpBattleConnection
                .SetConnection(port)
                .StartReceiveThread();
            mediator.SetUdpConnection(udpBattleConnection);
        }

        private static void StartGameRoomDeletingNotifierThread()
        {
            new Thread(() => new GameRoomDeletingNotifier().StartEndlessLoop().Wait()).Start();
        }
    }
}