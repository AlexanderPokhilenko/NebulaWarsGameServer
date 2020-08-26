using System.Net;
using System.Net.Sockets;
using Plugins.submodules.SharedCode.Logger;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.PlayerToServer;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;
using Server.Udp.Sending;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PingMessageHandler:IMessageHandler
    {
        private readonly IUdpSender udpSender;
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(PingMessageHandler));

        public PingMessageHandler(IpAddressesStorage ipAddressesStorage, IUdpSender udpSender)
        {
            this.ipAddressesStorage = ipAddressesStorage;
            this.udpSender = udpSender;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            PlayerPingMessage mes = ZeroFormatterSerializer
                .Deserialize<PlayerPingMessage>(messageWrapper.SerializedMessage);
            ushort playerId = mes.TemporaryId;
            int matchId = mes.MatchId;
            TryUpdateIpEndPoint(sender, matchId, playerId);
            PingAnswerMessage pingAnswerMessage = new PingAnswerMessage(mes.PingMessageId);
            byte[] data = ZeroFormatterSerializer.Serialize(pingAnswerMessage);
            MessageWrapper answer = new MessageWrapper()
            {
                MessageType = MessageType.PingAnswerMessage,
                SerializedMessage = data
            };
            var serializedMessageWrapper = ZeroFormatterSerializer.Serialize(answer);

            var messagesPack = new MessagesPack
            {
                Messages = new[] {serializedMessageWrapper}
            };

            byte[] serializedMessagePack = ZeroFormatterSerializer.Serialize(messagesPack);
            udpSender.Send(serializedMessagePack, sender);
        }

        private void TryUpdateIpEndPoint(IPEndPoint ipEndPoint, int matchId, ushort playerId)
        {
            bool successUpdate = ipAddressesStorage.TryUpdateIpEndPoint(matchId, playerId, ipEndPoint);
        }
    }
}