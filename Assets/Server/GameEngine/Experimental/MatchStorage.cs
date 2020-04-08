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
    /// Управляет всеми текущими матчами.
    /// </summary>
    public class MatchStorage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorage));
        
        
        //MatchId Match
        private readonly ConcurrentDictionary<int, Match> matches;
        //accountId Match
        private readonly ConcurrentDictionary<int, Match> activePlayers;

        public MatchStorage()
        {
            matches = new ConcurrentDictionary<int, Match>();
            activePlayers = new ConcurrentDictionary<int, Match>();
        }
        
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

        public void CreateMatch(BattleRoyaleMatchData matchData)
        {
            Match match = new Match(matchStorageFacade, matchData.MatchId);
            match.ConfigureSystems(matchData);
            matches.TryAdd(match.matchData.MatchId, match);
            foreach (var player in match.matchData.GameUnitsForMatch.Players)
            {
                Log.Info($"Добавление игрока к списку активных игроков {nameof(player.TemporaryId)} " +
                         $"{player.TemporaryId}");
                activePlayers.TryAdd(player.TemporaryId, match);
            }
        }


        public List<int> GetActivePlayersIds(int matchId)
        {
            List<int> activePlayersIds = matches[matchId].matchData
                .GameUnitsForMatch
                .Players
                .Select(player => player.TemporaryId)
                .Where(playerId => activePlayers.ContainsKey(playerId) 
                                   && activePlayers[playerId].matchData.MatchId==matchId)
                .ToList();
            
            foreach (var activePlayerId in activePlayersIds)
            {
                Log.Warn($"{nameof(GetActivePlayersIds)}" +
                         $" {nameof(matchId)} {matchId} " +
                         $"{nameof(activePlayerId)} {activePlayerId}");
            }
            return activePlayersIds;
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

        /// <summary>
        /// Удаляет игрока нахуй.
        /// </summary>
        public bool TryRemovePlayer(int matchId, int playerId)
        {
            Log.Warn($"{nameof(TryRemovePlayer)} {nameof(matchId)} {matchId} {nameof(playerId)} {playerId}");
            
            //Игрок есть в списке активных игроков?
            if (activePlayers.ContainsKey(playerId))
            {
                Log.Info("Игрок есть в списке активных игроков");
                if (activePlayers[playerId].matchData.MatchId == matchId)
                {
                    Log.Info($"Матч совпадает.");
                    
                    if (activePlayers.TryRemove(playerId, out Match match))
                    {
                        Log.Info($"Успешное удаление из списка активных игроков.");
                        if (match.TryRemovePlayerIpEndPoint(playerId))
                        {
                            Log.Info($"Успешное удаление ip адреса.");
                            return true;
                        }
                        else
                        {
                            Log.Info($"Не удалось удалить ip адрес");
                        }
                    }
                    else
                    {
                        Log.Info($"Не удалось удалить из списка активных игроков.");
                    }
                }
                else
                {
                    Log.Error($"Матч не совпадает. {activePlayers[playerId].matchData.MatchId} {matchId}");
                }
            }
            else
            {
                Log.Warn("Игрока не в списке активных игроков.");
            }
            return false;
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

        public bool TryAddEndPoint(int matchId, int playerId, IPEndPoint ipEndPoint)
        {
            Log.Warn($"Добавление ip адреса {nameof(matchId)} {matchId} {nameof(playerId)} {playerId}");
            if (matches.ContainsKey(matchId))
            {
                //TODO удалить ip этого игрока из других матчей
                foreach (var pair in matches)
                {
                    var match = pair.Value;
                    if (match.matchData.MatchId != matchId)
                    {
                        if (match.ContainsIpEnpPointForPlayer(playerId))
                        {
                            if (match.TryRemovePlayerIpEndPoint(playerId))
                            {
                                Log.Warn("Эта ебаная хуйня сработала. ЕБОЙ");
                            }
                        }
                    }
                }
                
                
                matches[matchId].AddEndPoint(playerId, ipEndPoint);
                return true;
            }
            else
            {
                Log.Error($"Такого матча не существует. {nameof(matchId)} = {matchId}");
                return false;
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