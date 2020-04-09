using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Обрабатывает подтверждение доставки. Удаляет сообщение с этим номераом из памяти, чтобы его больше не
    /// отправлять.
    /// </summary>
    public class RudpConfirmationReceiver:IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RudpConfirmationReceiver));
        
        private readonly MatchStorage matchStorage;
        private readonly ByteArrayRudpStorage byteArrayRudpStorage;

        public RudpConfirmationReceiver(MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage)
        {
            this.matchStorage = matchStorage;
            this.byteArrayRudpStorage = byteArrayRudpStorage;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            DeliveryConfirmationMessage mes =
                ZeroFormatterSerializer.Deserialize<DeliveryConfirmationMessage>(messageWrapper.SerializedMessage);
            uint messageIdToConfirm = mes.MessageNumberThatConfirms;
            byteArrayRudpStorage.TryRemoveMessage(messageIdToConfirm);
        }
    }
}