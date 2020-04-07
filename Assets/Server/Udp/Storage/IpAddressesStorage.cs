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
        private readonly ILog Log = LogManager.GetLogger(typeof(IpAddressesStorage));
        
        /// <summary>
        /// playerId , ip 
        /// </summary>
        private readonly ConcurrentDictionary<int, IPEndPoint> playersIpAddresses;

        public IpAddressesStorage()
        {
            playersIpAddresses = new ConcurrentDictionary<int, IPEndPoint>();
        }

        public bool IsIpAddressAlreadyExists(IPEndPoint point)
        {
            return playersIpAddresses.Values.Contains(point);
        }
        
        public void AddPlayer(int playerId, IPEndPoint sender)
        {
            if (playersIpAddresses.TryAdd(playerId, sender))
            {
                Log.Info($"Добавлен клиент с id={playerId}");
            }
            else
            {
                throw new Exception("Не удалось добавить игрока с playerId = "+playerId);
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