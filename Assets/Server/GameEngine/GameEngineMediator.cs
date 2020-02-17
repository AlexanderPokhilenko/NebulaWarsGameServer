using Server.GameEngine.StaticMessageSorters;
using Server.Udp.Sending;
using Server.Udp.Storage;
using UnityEngine;

namespace Server.GameEngine
{
    public class GameEngineMediator
    {
        public static readonly BattlesStorage BattlesStorage = new BattlesStorage();
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
            foreach (var gameSession in BattlesStorage.GetAllGameSessions())
            {
                gameSession.Execute();
                gameSession.Cleanup();
            }
            PingLogger.Log();
            SendUnconfirmedMessages();
            BattlesStorage.UpdateGameSessionsState();
        }

        //TODO вынести
        private void SendUnconfirmedMessages()
        {
            foreach (var playerId in BattlesStorage.PlayersToSessions.Keys)
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