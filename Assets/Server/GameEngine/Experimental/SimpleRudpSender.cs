using Server.Udp.Sending;
using Server.Udp.Storage;
using Server.Utils;
using UnityEngine;

namespace Server.GameEngine.Experimental
{
    public class SimpleRudpSender:IRudpSender
    {
        private readonly BattlesStorage battlesStorage;

        public SimpleRudpSender(BattlesStorage battlesStorage)
        {
            this.battlesStorage = battlesStorage;
        }

        public void SendUnconfirmedMessages()
        {
            foreach (var playerId in battlesStorage.playerToBattle.Keys)
            {
                var messages = ByteArrayRudpStorage.Instance.GetReliableMessages(playerId);
                if (messages != null && messages.Count!=0)
                {
                    Log.Error("Повторная отправка rudp. Кол-во сообщений = "+messages.Count);
                    foreach (var message in messages)
                    {
                        UdpSendUtils.SendMessage(message, playerId);
                    }
                }
            }
        }
    }
}