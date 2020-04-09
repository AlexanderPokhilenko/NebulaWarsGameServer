using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    /// <summary>
    /// Хранит таблицу текущих матчей и игроков
    /// </summary>
    public class MatchStorage
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MatchStorage));
        
        //matchId match
        private readonly ConcurrentDictionary<int, Match> matches;
      
        public MatchStorage()
        {
            matches = new ConcurrentDictionary<int, Match>();
        }

        public void AddMatch(Match match)
        {
            if (matches.TryAdd(match.MatchId, match))
            {
                //намана
            }
            else
            {
                //Возможно матч с таким id уже добавлен
                //это фиаско
                throw new Exception("Не удалось добавить матч в коллекцию.");
            }
        }
        
        /// <summary>
        /// Просто удаление матча из коллекции
        /// </summary>
        public Match RemoveMatch(int matchId)
        {
            if (matches.TryRemove(matchId, out Match match))
            {
                //намана   
                log.Info($"Матч удалён {nameof(matchId)} {matchId}.");
                return match;
            }
            else
            {
                //не намана
                throw new Exception($"Попытка удалить матч, которого нет {nameof(matchId)} {matchId}");
            }
        }

        /// <summary>
        /// Перед созданием матча.
        /// </summary>
        public bool HasPlayer(int playerId)
        {
            foreach (Match match in matches.Values)
            {
                if (match.GetActivePlayersIds().Contains(playerId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Перед созданием матча.
        /// </summary>
        public bool HasMatch(int matchId)
        {
            return matches.ContainsKey(matchId);
        }

        public IEnumerable<Match> GetAllMatches()
        {
            return matches.Values;
        }
        
        /// <summary>
        /// Получение матча для создания сущностей ввода.
        /// </summary>
        public bool TryGetMatchByPlayerId(int playerId, out Match match)
        {
            //TODO  пробежаться по активным игрокам в каждом матче
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновление ip адреса игрока.
        /// </summary>
        public bool TryUpdateIpEndPoint(int matchId, int playerId, IPEndPoint ipEndPoint)
        {
            if (matches.TryGetValue(matchId, out var match))
            {
                return match.TryUpdateIpEndPoint(playerId, ipEndPoint);
            }
            else
            {
                throw new Exception(nameof(TryUpdateIpEndPoint));
            }
        }

        /// <summary>
        /// Получение ip адреса для отправки сообщения.
        /// </summary>
        public bool TryGetIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            if (matches.TryGetValue(matchId, out var match))
            {
                if (match.TryGetIpEndPoint(playerId, out ipEndPoint))
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
    }
}