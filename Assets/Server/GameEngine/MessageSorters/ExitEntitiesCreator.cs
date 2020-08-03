using System.Collections.Concurrent;
using System.Collections.Generic;
using Code.Common;
using Server.GameEngine.MatchLifecycle;

namespace Server.GameEngine.MessageSorters
{
    /// <summary>
    /// Создаёт сообщения о преждевременном покидании боя в контекстах.
    /// </summary>
    public class ExitEntitiesCreator
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(ExitEntitiesCreator));
        
        //matchId, playerId
        private readonly ConcurrentDictionary<int, ushort> dictionary = new ConcurrentDictionary<int, ushort>();
        private readonly MatchStorage matchStorage;
        
        public ExitEntitiesCreator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public void AddExitMessage(int matchId, ushort playerId)
        {
            log.Info(nameof(AddExitMessage));
            if (!dictionary.ContainsKey(matchId))
            {
                dictionary.TryAdd(matchId, playerId);
            }
        }

        public void Create()
        {
            foreach (KeyValuePair<int, ushort> pair in dictionary)
            {
                int matchId = pair.Key;
                ushort playerId = pair.Value;


                if (matchStorage.TryGetMatch(matchId, out Match match))
                {
                    match.AddInputEntity(playerId, inputEntity => inputEntity.isLeftTheGame = true);
                } 
            }   
            
            dictionary.Clear();
        }
    }
}