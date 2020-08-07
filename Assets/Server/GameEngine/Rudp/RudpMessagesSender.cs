using System.Collections.Generic;
using System.Net;
using Code.Common;
using Server.GameEngine.MatchLifecycle;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine.Rudp
{
    /// <summary>
    /// Достаёт сообщения из rudp хранилища и кладёт их в очередь на отправку.
    /// </summary>
    public class RudpMessagesSender
    {
        private readonly MatchStorage matchStorage;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly ByteArrayRudpStorage byteArrayRudpStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(RudpMessagesSender));

        public RudpMessagesSender(ByteArrayRudpStorage byteArrayRudpStorage, MatchStorage matchStorage, 
            UdpSendUtils udpSendUtils, IpAddressesStorage ipAddressesStorage)
        {
            this.matchStorage = matchStorage;
            this.udpSendUtils = udpSendUtils;
            this.ipAddressesStorage = ipAddressesStorage;
            this.byteArrayRudpStorage = byteArrayRudpStorage;
        }
        
        public void SendAll()
        {
            List<(int matchId, ushort playerId)> pairs = new List<(int matchId, ushort playerId)>();
            
            //получить пары matchId playerId из хранилища матчей
            foreach (Match match in matchStorage.GetAllMatches())
            {
                int matchId = match.matchId;
                List<ushort> players = ipAddressesStorage.GetActivePlayersIds(matchId);
                if (players != null)
                {
                    foreach (ushort playersId in players)
                    {
                        pairs.Add((matchId, playersId));
                    }
                }
            }
            
            //запросить и отправить сообщения для всех пар
            foreach (var (matchId, playerId) in pairs)
            {
                //messageId, model
                byte[][] messagesForPlayer = byteArrayRudpStorage.GetMessages(matchId, playerId);
                if (messagesForPlayer != null)
                {
                    if(ipAddressesStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
                    {
                        foreach (byte[] data in messagesForPlayer)
                        {
                            udpSendUtils.SendReadyMadeMessage(matchId, playerId, data);
                        }
                    }
                    else
                    {
                        //такого быть не должно. ведь id игрока был получен из списка активных игроков
                        log.Error("ip игрока не найден, хотя он в списке активных игроков.");
                    }
                }
            }
        }
    }
}