using OldServer.Experimental.GameEngine.StaticMessageSorters;
using OldServer.Experimental.Udp.Sending;
using OldServer.Experimental.Udp.Storage;
using UnityEngine;

namespace OldServer.Experimental.GameEngine
{
    public class GameEngineMediator
    {
        public static readonly GameSessionsStorage GameSessionsStorage = new GameSessionsStorage();
        private readonly Clock clock;

        public GameEngineMediator()
        {
#if UNITY_5_3_OR_NEWER
            var go = new GameObject("Clock");
            clock = go.AddComponent<Clock>();
            clock.gameEngineMediator = this;
#else
            clock = new Clock(this);
#endif
        }

        public void Tick()
        {
            StaticInputMessagesSorter.Spread();
            foreach (var gameSession in GameSessionsStorage.GetAllGameSessions())
            {
                gameSession.Execute();
                gameSession.Cleanup();
            }
            PingLogger.Log();
            SendUnconfirmedMessages();
            GameSessionsStorage.UpdateGameSessionsState();
        }

        //TODO вынести
        private void SendUnconfirmedMessages()
        {
            foreach (var playerId in GameSessionsStorage.PlayersToSessions.Keys)
            {
                var messages = RudpStorage.GetReliableMessages(playerId);
                if (messages != null && messages.Count!=0)
                {
                    Debug.LogError("Повторная отправка rudp. Кол-во сообщений = "+messages.Count);
                    foreach (var message in messages)
                    {
                        UdpSendUtils.SendMessage(message, playerId);
                    }
                }
            }
        }

        public void StartEndlessLoop()
        {
            clock.StartEndlessLoop();
        }
    }
}