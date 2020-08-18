using System;
using System.Collections.Concurrent;
using Code.Common;
using Server.GameEngine.MatchLifecycle;
using SharedSimulationCode;
using SharedSimulationCode.Systems;

namespace Server.GameEngine.MessageSorters
{
    /// <summary>
    /// Создаёт сообщения о преждевременном покидании боя в контекстах.
    /// </summary>
    public class ExitEntitiesCreator
    {
        private readonly MatchesStorage matchesStorage;
        private readonly ILog log = LogManager.CreateLogger(typeof(ExitEntitiesCreator));
        //matchId, playerId
        private readonly ConcurrentStack<Tuple<int, ushort>> stack = new ConcurrentStack<Tuple<int, ushort>>();
        
        public ExitEntitiesCreator(MatchesStorage matchesStorage)
        {
            this.matchesStorage = matchesStorage;
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
                InputReceiver inputReceiver = null;
                if (matchesStorage.TryGetMatchInputReceiver(matchId, ref inputReceiver))
                {
                    inputReceiver.AddExit(playerId);
                } 
            }   
            
            stack.Clear();
        }
    }
}