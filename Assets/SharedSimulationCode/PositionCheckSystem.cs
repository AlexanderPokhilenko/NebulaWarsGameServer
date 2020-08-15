using System;
using Entitas;
using UnityEngine;

namespace SharedSimulationCode
{
    public class PositionCheckSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> transformGroup;

        public PositionCheckSystem(Contexts contexts)
        {
            transformGroup = contexts.game.GetGroup(GameMatcher.Transform);
        }
        
        public void Execute()
        {
            foreach (var entity in transformGroup)
            {
                if (Mathf.Abs(entity.transform.value.position.y) > 0.1f)
                {
                    throw new Exception("y != 0 "+entity.transform.value.position.y);
                }
            }    
        }
    }
}