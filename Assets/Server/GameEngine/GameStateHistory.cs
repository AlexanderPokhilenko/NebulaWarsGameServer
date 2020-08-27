using System.Collections.Generic;
using System.Linq;
using Plugins.submodules.SharedCode.LagCompensation;

namespace Server.GameEngine
{
    public class GameStateHistory : IGameStateHistory
    {
        private readonly Dictionary<int, SerializedGameState> history = new Dictionary<int, SerializedGameState>();
        
        public SerializedGameState Get(int tickNumber)
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

        public void Add(SerializedGameState gameState)
        {
            history.Add(gameState.tickNumber, gameState);
        }

        public SerializedGameState GetActualGameState()
        {
            return history[GetLastTickNumber()];
        }

        public float GetLastTickTime()
        {
            return history[GetLastTickNumber()].tickMatchTimeSec;
        }
    }
}