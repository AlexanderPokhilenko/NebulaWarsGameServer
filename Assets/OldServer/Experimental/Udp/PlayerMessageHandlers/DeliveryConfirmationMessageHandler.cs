using System.Net;
using Libraries.NetworkLibrary.Udp.PlayerToServer.ReliableUdp;
using NetworkLibrary.NetworkLibrary.Udp;
using OldServer.Experimental.Udp.ReliableUdp;
using ZeroFormatter;

namespace OldServer.Experimental.Udp.PlayerMessageHandlers
{
    public class DeliveryConfirmationMessageHandler:IMessageHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            DeliveryConfirmationMessage mes =
                ZeroFormatterSerializer.Deserialize<DeliveryConfirmationMessage>(message.SerializedMessage);
            int playerId = mes.PlayerId;
            uint messageIdToConfirm = mes.MessageNumberThatConfirms;

            ReliableUdpMessagesStorage.RemoveMessage(playerId, messageIdToConfirm);
        }
    }
}