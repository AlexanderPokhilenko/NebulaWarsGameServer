using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Code.Common;
using JetBrains.Annotations;
using NetworkLibrary.NetworkLibrary.Http;

namespace Server.Udp.Storage
{
/*    
    Активный игрок - игрок, которому нужно отправлять сообщения.
    У него ещё может не быть ip адреса.
    Он может быть убитым.
    
    Активноть игрока можно определить по наличию его в структуре данных с ip адресами.
    ip адрес может быть null. 
 */

    /// <summary>
    /// Содержит таблицу ip адресов для игроков.
    /// </summary>
    public class IpAddressesStorage
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(IpAddressesStorage));

        //TODO какую тут коллекцию выбрать?
        //matchId, playerId , ip
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<ushort, IPEndPoint>> ipEndPoints 
            = new ConcurrentDictionary<int, ConcurrentDictionary<ushort, IPEndPoint>>();
        
        public void AddMatch(BattleRoyaleMatchModel matchModel)
        {
            ConcurrentDictionary<ushort, IPEndPoint> playersIpAddresses = new ConcurrentDictionary<ushort, IPEndPoint>();
            foreach (PlayerModel playerModel in matchModel.GameUnits.Players)
            {
                playersIpAddresses.TryAdd(playerModel.TemporaryId, new IPEndPoint(1, 1));
            }
            ipEndPoints.TryAdd(matchModel.MatchId, playersIpAddresses);
        }

        public bool TryGetIpEndPoint(int matchId, ushort playerId, out IPEndPoint ipEndPoint)
        {
            if (ipEndPoints.TryGetValue(matchId, out var dictionary))
            {
                if (dictionary.TryGetValue(playerId, out ipEndPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                ipEndPoint = null;
                return false;
            }
        }

        public bool TryRemoveIpEndPoint(int matchId, ushort playerId)
        {
            bool success = ipEndPoints[matchId].TryRemove(playerId, out _);
            log.Warn($"Удаление ip для игрока {nameof(playerId)} {playerId}" );
            return success;
        }

        public bool TryUpdateIpEndPoint(int matchId, ushort playerId, IPEndPoint newIpEndPoint)
        {
            //игрок есть в списке активных игроков?
            if (ipEndPoints.TryGetValue(matchId, out var dictionary))
            {
                if (dictionary.TryGetValue(playerId, out var ipEndPoint))
                {
                    if (!Equals(newIpEndPoint, ipEndPoint))
                    {
                        return dictionary.TryUpdate(playerId, newIpEndPoint, ipEndPoint);
                    }
                    else
                    {
                        //ip игрока не поменялся
                        return false;
                    }   
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        [CanBeNull]
        public List<ushort> GetActivePlayersIds(int matchId)
        {
            if (ipEndPoints.TryGetValue(matchId, out var dictionary))
            {
                return dictionary.Keys.ToList();
            }

            return null;
        }

        public bool Contains(int matchId, ushort playerId)
        {
            return ipEndPoints.ContainsKey(matchId) && ipEndPoints[matchId].ContainsKey(playerId);
        }
    }
}