using System.Collections.Generic;
using System.Net;
using log4net;

namespace Server.GameEngine
{
    public class MatchStorageFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorageFacade));

        private readonly MatchStorage matchStorage;

        public bool ContainsIpEndPoint(int matchId, int playerId)
        {
            return matchStorage.ContainsIpEndPoint(matchId, playerId);
        }
        
        public bool TryAddEndPoint(int matchId, int playerId, IPEndPoint ipEndPoint)
        {
            return matchStorage.TryAddEndPoint(matchId, playerId, ipEndPoint);
        }
        
        public MatchStorageFacade()
        {
            matchStorage = new MatchStorage(this);
        }

        public ICollection<Match> GetAllMatches()
        {
            return matchStorage.DichGetMatches();
        }
        
        public bool TryRemovePlayer(int matchId, int playerId)
        {
            bool success = matchStorage.TryRemovePlayer(matchId, playerId);
            return success;
        }
        
        public bool HasMatchWithId(int matchId)
        {
            return matchStorage.HasMatchWithId(matchId);
        }
        
        public bool HasPlayerWithId(int playerId)
        {
            return matchStorage.HasPlayerWithId(playerId);
        }
        
        public bool TryGetMatchByPlayerId(int playerId, out Match match)
        {
            return matchStorage.TryGetMatchByPlayerId(playerId, out match);
        }

        public bool TryGetPlayerIpEndPoint(int matchId, int playerId, out IPEndPoint ipEndPoint)
        {
            return matchStorage.TryGetPlayerIpEndPoint(matchId, playerId, out ipEndPoint);
        }
        
        public void AddReliableMessage(int matchId, int playerId, uint messageId, byte[] serializedMessage)
        {
            matchStorage.AddReliableMessage(matchId, playerId, messageId, serializedMessage);
        }

        public void RemoveRudpMessage(uint messageIdToConfirm)
        {
            matchStorage.RemoveRudpMessage(messageIdToConfirm);
        }
    }

    
}