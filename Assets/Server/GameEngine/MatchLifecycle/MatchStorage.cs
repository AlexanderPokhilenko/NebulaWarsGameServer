using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using Code.Common;

namespace Server.GameEngine
{
    /// <summary>
    /// Хранит таблицу текущих матчей и игроков
    /// </summary>
    public class MatchStorage
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchStorage));
        
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
        /// Удаляет матч из коллекции.
        /// </summary>
        public Match DequeueMatch(int matchId)
        {
            if (matches.TryRemove(matchId, out Match match))
            {
                log.Info($"Матч удалён {nameof(matchId)} {matchId}.");
                return match;
            }
            else
            {
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
                if (match.HasPlayer(playerId))
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
        public bool TryGetMatchByPlayerId(int playerId, out Match matchArg)
        {
            foreach (Match match in matches.Values)
            {
                if (match.HasPlayer(playerId))
                {
                    matchArg = match;
                    return true;
                }
            }

            matchArg = null;
            return false;
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
                //TODO разобраться с этим
                return false;
                //
                // throw new Exception(nameof(TryUpdateIpEndPoint));
            }
        }

        /// <summary>
        /// Получение ip адреса для отправки сообщения.
        /// </summary>
        public bool TryGetIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            if (matches.TryGetValue(matchId, out var match))
            {
                return match.TryGetIpEndPoint(playerId, out ipEndPoint);
            }
            else
            {
                ipEndPoint = null;
                return false;
            }
        }
    }
}