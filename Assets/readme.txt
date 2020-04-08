zfc.exe -i "..\..\..\Assembly-CSharp.csproj" -o "ZeroFormatterGenerated.cs"

Что делать с сообщениями об удалении боя?
Ведь отправка rudp внутри матча.


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

        public ICollection<Match> GetAllMatches()
        {
            return matches.Values;
        }

      
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