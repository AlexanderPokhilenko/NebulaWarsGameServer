using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Sending;


namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Отправляет подтверждение доставки.
    /// </summary>
    public class RudpConfirmationSender:IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RudpConfirmationSender));
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            Log.Info("пришло rudp");
            DeliveryConfirmationMessage mes = new DeliveryConfirmationMessage
            {
                MessageNumberThatConfirms = messageWrapper.MessageId
            };
            UdpDich.SendDeliveryConfirmationMessage(mes, sender);
        }
    }
}