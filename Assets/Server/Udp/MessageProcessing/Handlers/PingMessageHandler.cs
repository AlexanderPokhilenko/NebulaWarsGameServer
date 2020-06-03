using System.Net;
using Code.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping;
using Server.GameEngine;
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
            
            int playerId = mes.TemporaryId;
            int matchId = mes.MatchId;
            
            TryUpdateIpEndPoint(sender,matchId, playerId);
        }

        private void TryUpdateIpEndPoint(IPEndPoint ipEndPoint, int matchId, int playerId)
        {
            bool successUpdate = ipAddressesStorage.TryUpdateIpEndPoint(matchId, playerId, ipEndPoint);
        }
    }
}