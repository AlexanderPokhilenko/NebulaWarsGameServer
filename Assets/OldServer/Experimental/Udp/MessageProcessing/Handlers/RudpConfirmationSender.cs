using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using OldServer.Experimental.Udp.Sending;
using UnityEngine;

namespace OldServer.Experimental.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Отправляет сообщение с подтверждением доставки
    /// </summary>
    public class RudpConfirmationSender:IMessageHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            Debug.LogError("пришло rudp");
            DeliveryConfirmationMessage mes = new DeliveryConfirmationMessage
            {
                MessageNumberThatConfirms = message.MessageId
            };
            UdpSendUtils.SendDeliveryConfirmationMessage(mes, sender);
        }
    }
}