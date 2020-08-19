using System.Net;
using Plugins.submodules.SharedCode.Logger;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.PlayerToServer;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PingMessageHandler:IMessageHandler
    {
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(PingMessageHandler));
        

        public PingMessageHandler(IpAddressesStorage ipAddressesStorage)
        {
            this.ipAddressesStorage = ipAddressesStorage;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            PlayerPingMessage mes = 
                ZeroFormatterSerializer.Deserialize<PlayerPingMessage>(messageWrapper.SerializedMessage);
            
            var playerId = mes.TemporaryId;
            var matchId = mes.MatchId;
            
            TryUpdateIpEndPoint(sender, matchId, playerId);
        }

        private void TryUpdateIpEndPoint(IPEndPoint ipEndPoint, int matchId, ushort playerId)
        {
            bool successUpdate = ipAddressesStorage.TryUpdateIpEndPoint(matchId, playerId, ipEndPoint);
        }
    }
}