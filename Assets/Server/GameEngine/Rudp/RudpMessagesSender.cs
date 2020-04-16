using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net;
using Server.Udp.Sending;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    public class RudpMessagesSender
    {
        private readonly ILog log = LogManager.GetLogger(typeof(RudpMessagesSender));
        
        private readonly ByteArrayRudpStorage byteArrayRudpStorage;
        private readonly MatchStorage matchStorage;

        public RudpMessagesSender(ByteArrayRudpStorage byteArrayRudpStorage, MatchStorage matchStorage)
        {
            this.byteArrayRudpStorage = byteArrayRudpStorage;
            this.matchStorage = matchStorage;
        }
        
        public void SendAll()
        {
            List<(int matchId, int playerId)> pairs = new List<(int matchId, int playerId)>();
            
            //получить пары matchId playerId из хранилища матчей
            foreach (Match match in matchStorage.GetAllMatches())
            {
                int matchId = match.MatchId;
                foreach (var playersId in match.GetActivePlayersIds())
                {
                    pairs.Add((matchId, playersId));
                }
            }
            
            //запросить и отправить сообщения для всех пар
            foreach ((int matchId, int playerId) in pairs)
            {
                //messageId, data
                byte[][] messagesForPlayer = byteArrayRudpStorage.GetMessages(matchId, playerId);
                
                if (messagesForPlayer != null)
                {
                    if(matchStorage.TryGetIpEndPoint(matchId, playerId, out IPEndPoint ipEndPoint))
                    {
                        for (int i = 0; i < messagesForPlayer.Length; i++)
                        {
                            var data = messagesForPlayer[i];
                            UdpSendUtils.SendReadyMadeMessage(data, ipEndPoint);
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