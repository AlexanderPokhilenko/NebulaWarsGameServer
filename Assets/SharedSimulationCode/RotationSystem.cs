using Entitas;
using UnityEngine;

namespace SharedSimulationCode
{
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
                float direction = inputEntity.attack.direction;
                ushort playerId = inputEntity.player.id;
                GameEntity playerEntity = gameContext.GetEntityWithPlayer(playerId);
                if (playerEntity == null)
                {
                    Debug.LogWarning($"Пришло сообщение о движении от игрока, которого (уже) нет в комнате. Данные игнорируются. {nameof(playerId)} {playerId}");
                    return;
                }

                if (playerEntity.hasRigidbody)
                {
                    const float rotationSpeed = 5f;
                    var eulerAngles = playerEntity.transform.value.eulerAngles;
                    
                    var desiredRotQ = Quaternion.Euler(eulerAngles.x, direction, eulerAngles.z);
                    var actualRot = Quaternion.Lerp(playerEntity.transform.value.rotation, desiredRotQ, Time.deltaTime * rotationSpeed);
                    
                    playerEntity.rigidbody.value.MoveRotation(actualRot);
                    
                    // Debug.LogError($"input x {playerJoystickInput.x} y {playerJoystickInput.x}");
                    // Debug.LogError($"force x {force.x} y {force.y} z {force.z}");
                }
            }
        }
    }
}