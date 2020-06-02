using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Code.Common;
using NetworkLibrary.NetworkLibrary.Http;

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
        private readonly int matchId;
        //PlayerId , ip 
        private readonly ConcurrentDictionary<int, IPEndPoint> playersIpAddresses;
        private readonly ILog log = LogManager.CreateLogger(typeof(IpAddressesStorage));

        public IpAddressesStorage(BattleRoyaleMatchData matchData)
        {
            matchId = matchData.MatchId;
            playersIpAddresses = new ConcurrentDictionary<int, IPEndPoint>();
            foreach (var playerInfo in matchData.GameUnitsForMatch.Players)
            {
                //рандомный ip для того, чтобы rudp записывались для игроков, которые ещё не подключились
                AddPlayer(playerInfo.AccountId, new IPEndPoint(11, 654));
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
            bool success = playersIpAddresses.TryRemove(playerId, out var ipEndPoint);
            log.Warn($"Удаление ip для игрока {nameof(playerId)} {playerId} {nameof(success)} {success}" );
            return success;
        }

        public bool TryUpdateIpEndPoint(int playerId, IPEndPoint newIpEndPoint)
        {
            // log.Info("Обновление ip для игрока "+playerId);
            //игрок есть в списке активных игроков?
            if (playersIpAddresses.TryGetValue(playerId, out IPEndPoint ipEndPoint))
            {
                if (!Equals(newIpEndPoint, ipEndPoint))
                {
                    //заменить устаревший адрес на актуальный
                    if (playersIpAddresses.TryUpdate(playerId, newIpEndPoint, ipEndPoint))
                    {
                        log.Info($"адрес нормально заменился {newIpEndPoint.Address} {newIpEndPoint.AddressFamily} {newIpEndPoint.Port}");
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

        public List<int> GetActivePlayersIds()
        {
            return playersIpAddresses.Keys.ToList();
        }

        public bool HasPlayer(int playerId)
        {
            return playersIpAddresses.Keys.Contains(playerId);
        }
    }
}