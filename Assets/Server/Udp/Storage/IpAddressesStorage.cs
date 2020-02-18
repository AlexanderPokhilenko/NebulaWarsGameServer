using System;
using System.Collections.Concurrent;
using System.Net;
using UnityEngine;

namespace Server.Udp.Storage
{
    public class IpAddressesStorage
    {
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
        
        public IPEndPoint GetPlayerIpAddress(int playerId)
        {
            if(playersIpAddresses.Keys.Contains(playerId))
            {
                return playersIpAddresses[playerId];
            }
            return null;
        }

        public bool TryRemovePlayerIp(int playerId)
        {
            return playersIpAddresses.TryRemove(playerId, out var ip);
        }
    }
}