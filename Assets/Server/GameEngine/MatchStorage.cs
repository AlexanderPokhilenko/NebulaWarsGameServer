using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net;
using Server.GameEngine.Experimental;

namespace Server.GameEngine
{
    /// <summary>
    /// Управляет всеми текущими матчами.
    /// </summary>
    public class MatchStorage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorage));
        
        //matchId Match
        private readonly ConcurrentDictionary<int, Match> matches;
        //accountId Match
        private readonly ConcurrentDictionary<int, Match> activePlayers;

        public bool ContainsIpEndPoint(int matchId, int playerId)
        {
            if (matches.ContainsKey(matchId))
            {
                return matches[matchId].ContainsIpEnpPointForPlayer(playerId);
            }
            else
            {
                return false;
            }
        }
        
        public MatchStorage()
        {
            matches = new ConcurrentDictionary<int, Match>();
            activePlayers = new ConcurrentDictionary<int, Match>();
        }

        public void AddMatch(Match match)
        {
            matches.TryAdd(match.matchData.MatchId, match);
            foreach (var player in match.matchData.GameUnitsForMatch.Players)
            {
                Log.Info($"Добавление игрока к списку активных игроков {nameof(player.TemporaryId)} " +
                         $"{player.TemporaryId}");
                activePlayers.TryAdd(player.TemporaryId, match);
            }
        }

        public IEnumerable<int> GetPlayersIds(int matchId)
        {
            var playersIds = matches[matchId].matchData
                .GameUnitsForMatch
                .Players
                .Select(player => player.TemporaryId);
            return playersIds;
        }
        
        public void RemoveMatch(int matchId)
        {
            Log.Info(nameof(RemoveMatch));
            matches.TryRemove(matchId, out var match);
            foreach (var playerInfoForMatch in match.matchData.GameUnitsForMatch.Players)
            {
                activePlayers.TryRemove(playerInfoForMatch.TemporaryId, out _);
            }
        }

        public void TearDownMatch(int matchId)
        {
            Log.Info(nameof(TearDownMatch));
            Match match = matches[matchId];
            match.TearDown();
        }

        public ICollection<Match> DichGetMatches()
        {
            return matches.Values;
        }

        public bool TryRemovePlayer(int playerTmpId)
        {
            Log.Info($"{nameof(TryRemovePlayer)} {playerTmpId}");
            bool success = activePlayers.TryRemove(playerTmpId, out _);
            return success;
        }

        public bool HasMatchWithId(int matchId)
        {
            return matches.ContainsKey(matchId);
        }

        public bool HasPlayerWithId(int playerId)
        {
            return activePlayers.ContainsKey(playerId);
        }
        
        public bool TryGetMatchByPlayerId(int playerId, out Match match)
        {
            return activePlayers.TryGetValue(playerId, out match);
        }

        public void AddEndPoint(int matchId, int playerId, IPEndPoint ipEndPoint)
        {
            if (matches.ContainsKey(matchId))
            {
                matches[matchId].AddEndPoint(playerId, ipEndPoint);
            }
            else
            {
                Log.Error("Такого боя не существует.");
            }
        }

        public bool TryGetPlayerIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            if(matches.ContainsKey(matchId))
            {
                return matches[matchId].TryGetPlayerIpEndPoint(playerId, out ipEndPoint);
            }
            else
            {
                ipEndPoint = null;
                return false;
            }
        }
        
        public void AddReliableMessage(int matchId, int playerId, uint messageId, byte[] serializedMessage)
        {
            if (matches.ContainsKey(matchId))
            {
                matches[matchId].AddReliableMessage(playerId, messageId, serializedMessage);
            }
            else
            {
                throw new Exception("asofvnoasiv");
            } 
        }

        public void RemoveRudpMessage(uint messageIdToConfirm)
        {
            foreach (var pair in matches)
            {
                Match match = pair.Value;
                if (match.TryRemoveRemoveRudpMessage(messageIdToConfirm))
                {
                    break;
                }
            }
        }

        public List<ReliableMessagesPack> GetActivePlayersRudpMessages()
        {
            List<ReliableMessagesPack> result = new List<ReliableMessagesPack>();
            foreach (var pair in matches)
            {
                Match match = pair.Value;
                List<ReliableMessagesPack> reliableMessagesPacksFromMatch = match.GetActivePlayersRudpMessages();
                result.AddRange(reliableMessagesPacksFromMatch);
            }

            return result;
        }
    }
}