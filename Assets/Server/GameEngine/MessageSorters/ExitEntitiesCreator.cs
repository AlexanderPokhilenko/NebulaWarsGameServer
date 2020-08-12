using System;
using System.Collections.Concurrent;
using Code.Common;
using Server.GameEngine.MatchLifecycle;
using SharedSimulationCode;

namespace Server.GameEngine.MessageSorters
{
    /// <summary>
    /// Создаёт сообщения о преждевременном покидании боя в контекстах.
    /// </summary>
    public class ExitEntitiesCreator
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(ExitEntitiesCreator));
        
        //matchId, playerId
        private readonly ConcurrentStack<Tuple<int, ushort>> stack = new ConcurrentStack<Tuple<int, ushort>>();
        private readonly MatchStorage matchStorage;
        
        public ExitEntitiesCreator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public void AddExitMessage(int matchId, ushort playerId)
        {
            log.Info(nameof(AddExitMessage));
            stack.Push(new Tuple<int, ushort>(matchId, playerId));
        }

        public void Create()
        {
            foreach (var pair in stack)
            {
                int matchId = pair.Item1;
                ushort playerId = pair.Item2;
                if (matchStorage.TryGetMatch(matchId, out MatchSimulation match))
                {
                    match.AddExit(playerId);
                } 
            }   
            
            stack.Clear();
        }
    }
}