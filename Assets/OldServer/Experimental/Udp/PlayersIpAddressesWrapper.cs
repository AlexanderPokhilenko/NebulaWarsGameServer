using System;
using System.Collections.Concurrent;
using System.Net;

namespace AmoebaBattleServer01.Experimental.Udp
{
    public class PlayersIpAddressesWrapper
    {
        /// <summary>
        /// key playerGoogleId 
        /// </summary>
        private readonly ConcurrentDictionary<int, IPEndPoint> playersIpAddresses;

        public PlayersIpAddressesWrapper()
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

        public void AddPlayer(int playerGoogleId, IPEndPoint sender)
        {
            while (!playersIpAddresses.TryAdd(playerGoogleId, sender))
            {
                
            }
            Console.WriteLine($"Добавлен клиент с id={playerGoogleId}");
        }
    }
}