using System.Threading;
using AmoebaBattleServer01.Experimental;
using AmoebaBattleServer01.Experimental.GameEngine;
using AmoebaBattleServer01.Experimental.Http;
using AmoebaBattleServer01.Experimental.Udp;

//TODO добавить удаление комнат
//TODO добавить защиту от чужих пингов
//TODO создание новых потоков выглядит не одинаково
//TODO заменить id игроков на числа
//TODO добавить возможность отсылать гейм матчеру состояние комнат

namespace AmoebaBattleServer01
{
    static class Program
    {
        static void Main()
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
            BentMediator mediator = new BentMediator();
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