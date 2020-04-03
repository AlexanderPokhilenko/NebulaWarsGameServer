using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Обрабатывает подтверждение доставки.
    /// </summary>
    public class RudpConfirmationReceiver:IMessageHandler
    {
        private readonly ByteArrayRudpStorage rudpStorage;
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerExitMessageHandler));

        public RudpConfirmationReceiver()
        {
            rudpStorage = ByteArrayRudpStorage.Instance;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            DeliveryConfirmationMessage mes =
                ZeroFormatterSerializer.Deserialize<DeliveryConfirmationMessage>(messageWrapper.SerializedMessage);
            uint messageIdToConfirm = mes.MessageNumberThatConfirms;
            // Log.Warn("Пришло уведомление о плучении сообщения с номером = "+messageIdToConfirm);
            rudpStorage.RemoveMessage(messageIdToConfirm);
        }
    }
}