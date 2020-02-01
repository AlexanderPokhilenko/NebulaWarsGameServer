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
        private readonly ConcurrentDictionary<string, IPEndPoint> playersIpAddresses;

        public PlayersIpAddressesWrapper()
        {
            playersIpAddresses = new ConcurrentDictionary<string, IPEndPoint>();
        }

        public IPEndPoint GetPlayerIpAddress(string playerGoogleId)
        {
            if(playersIpAddresses.Keys.Contains(playerGoogleId))
            {
                return playersIpAddresses[playerGoogleId];
            }
            return null;
        }
        
        public bool IsIpAddressAlreadyExists(IPEndPoint point)
        {
            return playersIpAddresses.Values.Contains(point);
        }

        public void AddPlayer(string playerGoogleId, IPEndPoint sender)
        {
            while (!playersIpAddresses.TryAdd(playerGoogleId, sender))
            {
                
            }
            Console.WriteLine($"Добавлен клиент с id={playerGoogleId}");
        }
    }
}