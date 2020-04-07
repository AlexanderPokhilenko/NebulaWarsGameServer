using System.Collections.Generic;
using System.Linq;
using System.Net;
using Server.Udp.Sending;

//TODO: попробовать разобраться без ToList - пока без него падает из-за модификации коллекции

namespace Server.GameEngine.Experimental
{
    public struct ReliableMessagesPack
    {
        // public int playerId;
        // public int matchId;
        public readonly IPEndPoint IpEndPoint;
        public readonly Dictionary<uint, byte[]>.ValueCollection reliableMessages;

        public ReliableMessagesPack(IPEndPoint ipEndPoint, Dictionary<uint, byte[]>.ValueCollection reliableMessages)
        {
            IpEndPoint = ipEndPoint;
            this.reliableMessages = reliableMessages;
        }
    }
    
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
            foreach (ReliableMessagesPack reliableMessagesPack in matchStorageFacade.GetActivePlayersRudpMessages().ToArray())
            {
                foreach (var message in reliableMessagesPack.reliableMessages.ToList())
                {
                    UdpSendUtils.SendReadyMadeMessage(message, reliableMessagesPack.IpEndPoint);
                }
            }
        }
    }
}