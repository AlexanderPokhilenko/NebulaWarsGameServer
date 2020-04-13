using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping;
using Server.GameEngine;
using Server.Udp.Connection;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PingMessageHandler:IMessageHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof(UdpListener));
        
        private readonly MatchStorage matchStorage;

        public PingMessageHandler(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            PlayerPingMessage mes = 
                ZeroFormatterSerializer.Deserialize<PlayerPingMessage>(messageWrapper.SerializedMessage);
            
            int playerId = mes.TemporaryId;
            int matchId = mes.GameRoomNumber;

            TryUpdateIpEndPoint(sender,matchId, playerId);
        }

        private void TryUpdateIpEndPoint(IPEndPoint ipEndPoint, int matchId, int playerId)
        {
            bool successUpdate = matchStorage.TryUpdateIpEndPoint(matchId, playerId, ipEndPoint);
        }
    }
}