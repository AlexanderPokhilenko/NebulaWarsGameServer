using System.Collections.Generic;
using System.Linq;
using Plugins.submodules.SharedCode.LagCompensation;

namespace Server.GameEngine
{
    public class GameStateHistory : IGameStateHistory
    {
        private readonly Dictionary<int, ServerGameState> history = new Dictionary<int, ServerGameState>();
        
        public ServerGameState Get(int tickNumber)
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

        public void Add(ServerGameState serverGameState)
        {
            history.Add(serverGameState.tickNumber, serverGameState);
        }

        public ServerGameState GetActualGameState()
        {
            return history[GetLastTickNumber()];
        }

        public float GetLastTickTime()
        {
            return history[GetLastTickNumber()].tickSimulationStartTime;
        }
    }
}