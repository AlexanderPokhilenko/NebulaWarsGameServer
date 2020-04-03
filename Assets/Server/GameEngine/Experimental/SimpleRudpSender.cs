using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine.Experimental
{
    /// <summary>
    /// Отправляет все сообщения, доставка которыъ не была подтверждена
    /// </summary>
    public class SimpleRudpSender:IRudpSender
    {
        private readonly MatchStorage matchStorage;

        public SimpleRudpSender(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }

        public void SendUnconfirmedMessages()
        {
            foreach (var playerId in matchStorage.playerToBattle.Keys)
            {
                var messages = ByteArrayRudpStorage.Instance.GetReliableMessages(playerId);
                if (messages != null && messages.Count != 0)
                {
                    foreach (var message in messages)
                    {
                        UdpSendUtils.SendMessage(message, playerId);
                    }
                }
            }
        }
    }
}