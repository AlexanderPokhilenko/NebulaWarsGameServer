using System.Linq;
using Entitas;
using Server.GameEngine.Experimental;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Отправляет заново все сообщения, доставка которых не была подтверждена для игрков в этом бою.
    /// </summary>
    public class RudpSendingSystem:IExecuteSystem
    {
        private readonly IRudpMessagesStorage rudpMessagesStorage;

        public RudpSendingSystem(IRudpMessagesStorage rudpMessagesStorage)
        {
            this.rudpMessagesStorage = rudpMessagesStorage;
        }
        public void Execute()
        {
            //TODO убрать ToList
            var messagesPacks = rudpMessagesStorage.GetMessagesForActivePlayers();
            if (messagesPacks != null)
            {
                foreach (ReliableMessagesPack reliableMessagesPack in messagesPacks)
                {
                    foreach (var message in reliableMessagesPack.reliableMessages.ToList())
                    {
                        UdpSendUtils.SendReadyMadeMessage(message, reliableMessagesPack.IpEndPoint);
                    }
                }    
            }
        }
    }
}