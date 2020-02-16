using System.Threading;
using AmoebaBattleServer01.Experimental.GameEngine;
using AmoebaBattleServer01.Experimental.Http;
using OldServer.Experimental;
using OldServer.Experimental.Udp;
using OldServer.Experimental.Udp.Connection;

//TODO добавить удаление комнат
//TODO добавить защиту от чужих пингов
//TODO создание новых потоков выглядит не одинаково
//TODO добавить возможность отсылать гейм матчеру состояние комнат

namespace OldServer
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
            udpBattleConnection.SetConnection(port);
            udpBattleConnection.StartReceiveThread();
            mediator.SetUdpConnection(udpBattleConnection);
        }

        private static void StartGameRoomDeletingNotifierThread()
        {
            new Thread(() => new GameRoomDeletingNotifier().StartEndlessLoop().Wait()).Start();
        }
    }
}