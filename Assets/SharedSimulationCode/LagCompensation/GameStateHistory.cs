using System.Collections.Generic;
using System.Linq;
using Libraries.Logger;

namespace SharedSimulationCode.LagCompensation
{
    public class GameStateHistory:IGameStateHistory
    {
        //todo добавить циклический буффер
        private readonly List<GameState> list = new List<GameState>(10*60);
        private readonly ILog log = LogManager.CreateLogger(typeof(GameStateHistory));
        
        public void Add(GameState gameState)
        {
            list.Add(gameState);
        }

        public GameState GetActualGameState()
        {
            return list.Last();
        }

        public GameState Get(int tickNumber)
        {
            return list[tickNumber];
        }
        
        public int GetLastTickNumber()
        {
            return list.Count-1;
        }
    }
}