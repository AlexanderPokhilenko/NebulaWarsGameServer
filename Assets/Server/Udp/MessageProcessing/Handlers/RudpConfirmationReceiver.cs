using System.Net;
using Code.Common;
using Code.Common.Logger;

using Libraries.NetworkLibrary.Udp.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine.Rudp;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Обрабатывает подтверждение доставки. Удаляет сообщение с этим номераом из памяти, чтобы его больше не
    /// отправлять.
    /// </summary>
    public class RudpConfirmationReceiver:IMessageHandler
    {
        private readonly ByteArrayRudpStorage byteArrayRudpStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(RudpConfirmationReceiver));

        public RudpConfirmationReceiver(ByteArrayRudpStorage byteArrayRudpStorage)
        {
            this.byteArrayRudpStorage = byteArrayRudpStorage;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            DeliveryConfirmationMessage mes =
                ZeroFormatterSerializer.Deserialize<DeliveryConfirmationMessage>(messageWrapper.SerializedMessage);
            uint messageIdToConfirm = mes.MessageNumberThatConfirms;
            int matchId = mes.MatchId;
            ushort playerId = mes.PlayerId;
            // log.Debug($"{matchId} {playerId} {messageIdToConfirm}");
            byteArrayRudpStorage.TryRemoveMessage(matchId, playerId, messageIdToConfirm);
        }
    }
}