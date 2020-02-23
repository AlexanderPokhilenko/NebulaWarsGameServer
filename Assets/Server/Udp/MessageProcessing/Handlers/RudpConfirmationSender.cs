using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Sending;
using Server.Utils;
using UnityEngine;

namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Отправляет подтверждение доставки.
    /// </summary>
    public class RudpConfirmationSender:IMessageHandler
    {
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            Log.Info("пришло rudp");
            DeliveryConfirmationMessage mes = new DeliveryConfirmationMessage
            {
                MessageNumberThatConfirms = messageWrapper.MessageId
            };
            UdpSendUtils.SendDeliveryConfirmationMessage(mes, sender);
        }
    }
}