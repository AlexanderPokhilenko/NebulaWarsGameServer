using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Обрабатывает подтверждение доставки
    /// </summary>
    public class DeliveryConfirmationMessageHandler:IMessageHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            DeliveryConfirmationMessage mes =
                ZeroFormatterSerializer.Deserialize<DeliveryConfirmationMessage>(message.SerializedMessage);
            
            uint messageIdToConfirm = mes.MessageNumberThatConfirms;

            RudpStorage.RemoveMessage(messageIdToConfirm);
        }
    }
}