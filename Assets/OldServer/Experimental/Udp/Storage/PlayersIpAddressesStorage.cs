using System;
using System.Collections.Concurrent;
using System.Net;

namespace OldServer.Experimental.Udp.Storage
{
    public class PlayersIpAddressesStorage
    {
        /// <summary>
        /// key playerId 
        /// </summary>
        private readonly ConcurrentDictionary<int, IPEndPoint> playersIpAddresses;

        public PlayersIpAddressesStorage()
        {
            playersIpAddresses = new ConcurrentDictionary<int, IPEndPoint>();
        }

        public IPEndPoint GetPlayerIpAddress(int playerId)
        {
            if(playersIpAddresses.Keys.Contains(playerId))
            {
                return playersIpAddresses[playerId];
            }
            return null;
        }
        
        public bool IsIpAddressAlreadyExists(IPEndPoint point)
        {
            return playersIpAddresses.Values.Contains(point);
        }

        public void AddPlayer(int playerId, IPEndPoint sender)
        {
            if (playersIpAddresses.TryAdd(playerId, sender))
            {
                Console.WriteLine($"Добавлен клиент с id={playerId}");
            }
            else
            {
                throw new Exception("Не удалось добавить игрока с playerId = "+playerId);
            }
        }
    }
}