using System;
using Entitas;
using UnityEngine;

namespace SharedSimulationCode.Systems.Check
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
                if (Mathf.Abs(entity.transform.value.position.y) > 0.3f)
                {
                    var position = entity.transform.value.position;
                    Debug.LogError($"entity.hasPlayer = "+entity.hasPlayer);
                    Debug.LogError($"position = {position.x} {position.y} {position.z}");
                    Debug.LogError($"position = {position.x} {position.y} {position.z}");
                    throw new Exception("y != 0 "+position.y);
                }
            }    
        }
    }
}