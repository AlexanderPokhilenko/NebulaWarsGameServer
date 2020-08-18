using System.Collections.Generic;
using Entitas;
using SharedSimulationCode.LagCompensation;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Добавляет новое состояние в историю
    /// </summary>
    public class GameStateHistoryUpdaterSystem : IExecuteSystem
    {
        private readonly IGameStateHistory history;
        private readonly IGroup<GameEntity> warshipsGroup;

        public GameStateHistoryUpdaterSystem(Contexts contexts, IGameStateHistory history)
        {
            this.history = history;
            warshipsGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Transform, GameMatcher.HealthPoints));
        }

        public void Execute()
        {
            int tickNumber = history.GetLastTickNumber();
            Dictionary<ushort, Transform> dictionary = new Dictionary<ushort, Transform>();
            foreach (var entity in warshipsGroup)
            {
                ushort id = entity.id.value;
                Transform transform = entity.transform.value;
                dictionary.Add(id, transform);
            }
            
            GameState gameState = new GameState(tickNumber, dictionary);
            history.Add(gameState);
        }
    }
}