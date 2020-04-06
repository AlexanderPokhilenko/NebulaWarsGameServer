using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Server.GameEngine
{
    /// <summary>
    /// Скрывает в себе данные про текущие матчи.
    /// </summary>
    public class MatchStorage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorage));
        
        //matchId Match
        private readonly ConcurrentDictionary<int, Match> matches;
        //accountId Match
        private readonly ConcurrentDictionary<int, Match> activePlayers;

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
                Log.Warn($"Добавление игрока к списку активных игроков {nameof(player.TemporaryId)} " +
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
            Log.Warn(nameof(RemoveMatch));
            matches.TryRemove(matchId, out var match);
            foreach (var playerInfoForMatch in match.matchData.GameUnitsForMatch.Players)
            {
                activePlayers.TryRemove(playerInfoForMatch.TemporaryId, out _);
            }
        }

        public void TearDownMatch(int matchId)
        {
            Log.Warn(nameof(TearDownMatch));
            Match match = matches[matchId];
            match.TearDown();
        }

        public ICollection<Match> DichGetMatches()
        {
            return matches.Values;
        }

        public bool TryRemovePlayer(int playerTmpId)
        {
            Log.Error($"{nameof(TryRemovePlayer)} {playerTmpId}");
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

        public ICollection<int> GetActivePlayerIds()
        {
            return activePlayers.Keys;
        }

        public bool TryGetMatchByPlayerId(int playerId, out Match match)
        {
            Log.Error("Вывод всех игроков");
            foreach (var pair in activePlayers)
            {
                Log.Error($"playerId = {pair.Key} matchId {pair.Value.matchData.MatchId}");
            }
            return activePlayers.TryGetValue(playerId, out match);
        }
    }
}