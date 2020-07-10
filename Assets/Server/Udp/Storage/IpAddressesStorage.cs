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
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, IPEndPoint>> ipEndPoints 
            = new ConcurrentDictionary<int, ConcurrentDictionary<int, IPEndPoint>>();
        
        public void AddMatch(BattleRoyaleMatchModel matchModel)
        {
            ConcurrentDictionary<int, IPEndPoint> playersIpAddresses = new ConcurrentDictionary<int, IPEndPoint>();
            foreach (PlayerModel playerModel in matchModel.GameUnits.Players)
            {
                playersIpAddresses.TryAdd(playerModel.AccountId, new IPEndPoint(1, 1));
            }
            ipEndPoints.TryAdd(matchModel.MatchId, playersIpAddresses);
        }

        public bool TryGetIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
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

        public bool TryRemoveIpEndPoint(int matchId, int playerId)
        {
            bool success = ipEndPoints[matchId].TryRemove(playerId, out _);
            log.Warn($"Удаление ip для игрока {nameof(playerId)} {playerId}" );
            return success;
        }

        public bool TryUpdateIpEndPoint(int matchId, int playerId, IPEndPoint newIpEndPoint)
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
        public List<int> GetActivePlayersIds(int matchId)
        {
            if (ipEndPoints.TryGetValue(matchId, out var dictionary))
            {
                return dictionary.Keys.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}