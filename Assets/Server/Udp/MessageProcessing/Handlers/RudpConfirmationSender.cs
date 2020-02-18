using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Sending;
using Server.Utils;
using UnityEngine;

namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Отправляет сообщение с подтверждением доставки
    /// </summary>
    public class RudpConfirmationSender:IMessageHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            Log.Error("пришло rudp");
            DeliveryConfirmationMessage mes = new DeliveryConfirmationMessage
            {
                MessageNumberThatConfirms = message.MessageId
            };
            UdpSendUtils.SendDeliveryConfirmationMessage(mes, sender);
        }
    }
}