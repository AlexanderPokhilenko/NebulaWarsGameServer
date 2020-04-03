using System.Linq;
using Server.Udp.Sending;
using Server.Udp.Storage;

//TODO как тут красиво получить список активных игроков?

namespace Server.GameEngine.Experimental
{
    /// <summary>
    /// Отправляет все сообщения, доставка которыъ не была подтверждена
    /// </summary>
    public class SimpleRudpSender:IRudpSender
    {
        private readonly MatchStorageFacade matchStorageFacade;

        public SimpleRudpSender(MatchStorageFacade matchStorageFacade)
        {
            this.matchStorageFacade = matchStorageFacade;
        }

        public void SendUnconfirmedMessages()
        {
            foreach (var playerId in matchStorageFacade.GetActivePlayerIds())
            {
                //TODO: попробовать разобраться без ToList - пока без него падает из-за модификации коллекции
                var messages = ByteArrayRudpStorage.Instance.GetReliableMessages(playerId)?.ToList();
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