using System;
using System.Collections.Concurrent;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEngine;

namespace Server.Udp.Storage
{
/*    
    Активный игрок - игрок, которому нужно отправлять сообщения.
    У него ещё может не быть ip адреса.
    У каждого матча игроки отдельные. 
    Если игрок покинул один матч, то может войти в другой на этом сервере.
    События в матчах никак не влияют друг на друга.

    Активноть игрока можно определить по наличию его в структуре данных с ip адресами.
    ip адрес может быть null. При смерти игрока или при преждевременном выходе он навсегда
    исключается из хранилища ip адресов матча. 
 */
    /// <summary>
    /// Содержит таблицу ip адресов для игроков.
    /// </summary>
    public class IpAddressesStorage
    {
        private readonly ILog log = LogManager.GetLogger(typeof(IpAddressesStorage));
        
        //PlayerId , ip 
        private readonly int matchId;
        private readonly ConcurrentDictionary<int, IPEndPoint> playersIpAddresses;

        public IpAddressesStorage(BattleRoyaleMatchData matchData)
        {
            matchId = matchData.MatchId;
            playersIpAddresses = new ConcurrentDictionary<int, IPEndPoint>();
            foreach (var playerInfo in matchData.GameUnitsForMatch.Players)
            {
                AddPlayer(playerInfo.AccountId, null);
            }
        }

        private void AddPlayer(int playerId, IPEndPoint sender)
        {
            if (playersIpAddresses.TryAdd(playerId, sender))
            {
                log.Info($"В словарь ip адресов для матча {matchId} добавлен игрок с id {playerId}");
            }
            else
            {
                string errMessage = $"Не удалось добавить игрока с PlayerId = {playerId} " +
                                    $"{nameof(matchId)} {matchId}";
                throw new Exception(errMessage);
            }
        }

        public bool TryGetIpEndPoint(int playerId, out IPEndPoint ipEndPoint)
        {
            return playersIpAddresses.TryGetValue(playerId, out ipEndPoint);
        }

        public bool TryRemoveIpEndPoint(int playerId)
        {
            return playersIpAddresses.TryRemove(playerId, out var ipEndPoint);
        }

        public bool TryUpdateIpEndPoint(int playerId, IPEndPoint newIpEndPoint)
        {
            //игрок есть в списке активных игроков?
            if (playersIpAddresses.TryGetValue(playerId, out IPEndPoint ipEndPoint))
            {
                //ip игрока поменялся?
                //например с null на настоящий
                //или если у мобильного оператора поменялись вышки
                if (!Equals(newIpEndPoint, ipEndPoint))
                {
                    //заменить устаревший адрес на актуальный
                    if (playersIpAddresses.TryUpdate(playerId, newIpEndPoint, ipEndPoint))
                    {
                        //адрес нормально заменился
                        return true;
                    }
                    else
                    {
                        //такого быть не должно
                        string errMessage = "По неизвестной причине не удалось поменять ip адрес " +
                                            "на более актуальный.";
                        throw new Exception(errMessage);
                    }
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
    }
}