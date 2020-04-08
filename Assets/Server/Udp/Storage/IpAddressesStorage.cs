using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using log4net;

namespace Server.Udp.Storage
{
    /// <summary>
    /// Содержит таблицу ip адресов для игроков
    /// </summary>
    public class IpAddressesStorage
    {
        private readonly int matchId;
        private readonly ILog log = LogManager.GetLogger(typeof(IpAddressesStorage));
        
        /// <summary>
        /// PlayerId , ip 
        /// </summary>
        private readonly ConcurrentDictionary<int, IPEndPoint> playersIpAddresses;

        public IpAddressesStorage(int matchId)
        {
            this.matchId = matchId;
            playersIpAddresses = new ConcurrentDictionary<int, IPEndPoint>();
        }
        
        public void AddPlayer(int playerId, IPEndPoint sender)
        {
            if (playersIpAddresses.TryAdd(playerId, sender))
            {
                log.Info($"В словарь ip адресов для матча {matchId} добавлен игрок с id={playerId}");
            }
            else
            {
                throw new Exception($"Не удалось добавить игрока с PlayerId = {playerId} " +
                                    $"{nameof(matchId)} {matchId}");
            }
        }

        public bool ContainsPlayerIpEndPoint(int playerId)
        {
            return playersIpAddresses.Keys.Contains(playerId);
        }
        
        public bool TryGetPlayerIpEndPoint(int playerId, out IPEndPoint ipEndPoint)
        {
            if(playersIpAddresses.Keys.Contains(playerId))
            {
                ipEndPoint= playersIpAddresses[playerId];
                return true;
            }
            else
            {
                ipEndPoint = null;
                return false;
            }
        }

        public bool TryRemovePlayerIp(int playerId)
        {
            return playersIpAddresses.TryRemove(playerId, out var ip);
        }

        public ConcurrentDictionary<int, IPEndPoint> GetPlayersRoutingData()
        {
            return playersIpAddresses;
        }
    }
}