using System.Collections.Generic;
using System.Linq;
using Plugins.submodules.SharedCode.LagCompensation;

namespace Server.GameEngine
{
    public class GameStateHistory : IGameStateHistory
    {
        private readonly Dictionary<int, FullSnapshot> history =
            new Dictionary<int, FullSnapshot>();
        
        public FullSnapshot Get(int tickNumber)
        {
            return history[tickNumber];
        }

        public int GetLastTickNumber()
        {
            if (history.Keys.Count == 0)
            {
                return -1;
            }
            
            return history.Keys.Max();
        }

        public void Add(FullSnapshot gameState)
        {
            history.Add(gameState.tickNumber, gameState);
        }

        public FullSnapshot GetActualGameState()
        {
            return history[GetLastTickNumber()];
        }

        public float GetLastTickTime()
        {
            return history[GetLastTickNumber()].tickMatchTimeSec;
        }
    }
}