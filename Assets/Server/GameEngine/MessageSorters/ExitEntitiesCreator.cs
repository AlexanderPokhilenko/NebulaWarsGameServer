using System.Collections.Concurrent;
using System.Collections.Generic;
using Code.Common;

namespace Server.GameEngine.Experimental
{
    /// <summary>
    /// Создаёт сообщения о преждевременном покидании боя в контекстах.
    /// </summary>
    public class ExitEntitiesCreator
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(ExitEntitiesCreator));
        
        //matchId, playerId
        private readonly ConcurrentDictionary<int, int> dictionary = new ConcurrentDictionary<int, int>();
        private readonly MatchStorage matchStorage;
        
        public ExitEntitiesCreator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public void AddExitMessage(int matchId, int playerId)
        {
            log.Info(nameof(AddExitMessage));
            if (!dictionary.ContainsKey(matchId))
            {
                dictionary.TryAdd(matchId, playerId);
            }
        }

        public void Create()
        {
            foreach (KeyValuePair<int, int> pair in dictionary)
            {
                int matchId = pair.Key;
                int playerId = pair.Value;


                if (matchStorage.TryGetMatch(matchId, out Match match))
                {
                    match.AddInputEntity(playerId, inputEntity => inputEntity.isLeftTheGame = true);
                } 
            }   
            
            dictionary.Clear();
        }
    }
}