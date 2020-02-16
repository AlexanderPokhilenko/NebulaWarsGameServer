using System.Net;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using NetworkLibrary.NetworkLibrary.Udp;
using OldServer.Experimental.Udp.Sending;

namespace OldServer.Experimental.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Отправляет сообщение с подтверждением доставки
    /// </summary>
    public class RudpConfirmationSender:IMessageHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            DeliveryConfirmationFromServerMessage mes = new DeliveryConfirmationFromServerMessage
            {
                MessageNumberThatConfirms = message.MessageId
            };
            UdpSendUtils.SendDeliveryConfirmationMessage(mes, sender);
        }
    }
}