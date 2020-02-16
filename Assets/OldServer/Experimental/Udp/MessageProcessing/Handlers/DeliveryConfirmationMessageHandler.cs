using System.Net;
using Libraries.NetworkLibrary.Udp.PlayerToServer.ReliableUdp;
using NetworkLibrary.NetworkLibrary.Udp;
using OldServer.Experimental.Udp.Storage;
using ZeroFormatter;

namespace OldServer.Experimental.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Обрабатывает сообщение о подтверждении доставки
    /// </summary>
    public class DeliveryConfirmationMessageHandler:IMessageHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            DeliveryConfirmationMessage mes =
                ZeroFormatterSerializer.Deserialize<DeliveryConfirmationMessage>(message.SerializedMessage);
            int playerId = mes.PlayerId;
            uint messageIdToConfirm = mes.MessageNumberThatConfirms;

            ReliableUdpStorage.RemoveMessage(playerId, messageIdToConfirm);
        }
    }
}