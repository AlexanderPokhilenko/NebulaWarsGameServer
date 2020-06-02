using System.Net;
using Code.Common;
using Libraries.NetworkLibrary.Udp.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Sending;

namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Отправляет подтверждение доставки.
    /// </summary>
    public class RudpConfirmationSender:IMessageHandler
    {
        private readonly UdpSendUtils udpSendUtils;
        private static readonly ILog Log = LogManager.CreateLogger(typeof(RudpConfirmationSender));

        public RudpConfirmationSender(UdpSendUtils udpSendUtils)
        {
            this.udpSendUtils = udpSendUtils;
        }
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            Log.Info("пришло rudp");
            DeliveryConfirmationMessage mes = new DeliveryConfirmationMessage
            {
                MessageNumberThatConfirms = messageWrapper.MessageId
            };
            udpSendUtils.SendDeliveryConfirmationMessage(mes, sender);
        }
    }
}