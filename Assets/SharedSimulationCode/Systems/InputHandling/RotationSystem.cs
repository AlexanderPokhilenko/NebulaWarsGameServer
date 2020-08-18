using Entitas;
using UnityEngine;

namespace SharedSimulationCode.Systems.InputHandling
{
    /// <summary>
    /// По вводу игрока поворачивает его с ограничением скорости
    /// </summary>
    public class RotationSystem : IExecuteSystem
    {
        private readonly GameContext gameContext;
        private readonly IGroup<InputEntity> inputGroup;

        public RotationSystem(Contexts contexts)
        {
            gameContext = contexts.game;
            inputGroup = contexts.input
                .GetGroup(InputMatcher.AllOf(InputMatcher.Movement, InputMatcher.Player));
        }
        
        public void Execute()
        {
            foreach (InputEntity inputEntity in inputGroup)
            {
                float desiredAngle = inputEntity.attack.direction;
                ushort playerId = inputEntity.player.id;
                GameEntity playerEntity = gameContext.GetEntityWithPlayer(playerId);
                if (playerEntity == null)
                {
                    Debug.LogError($"Нет такого игрока. {nameof(playerId)} {playerId}");
                    continue;
                }

                if (!playerEntity.hasRigidbody)
                {
                    Debug.LogError($"Нет rigidbody");
                    continue;
                }
                
                if (float.IsNaN(desiredAngle))
                {
                    playerEntity.rigidbody.value.angularVelocity = Vector3.zero;
                    continue;
                }
                
                float angularVelocity = 15;
                Quaternion currentRotation = playerEntity.rigidbody.value.rotation;
                Quaternion desiredRotation = Quaternion.Euler(0,desiredAngle,0);
                Quaternion actualRotQ = Quaternion
                    .RotateTowards(currentRotation, desiredRotation, angularVelocity); 
                playerEntity.rigidbody.value.MoveRotation(actualRotQ);
            }
        }
    }
}