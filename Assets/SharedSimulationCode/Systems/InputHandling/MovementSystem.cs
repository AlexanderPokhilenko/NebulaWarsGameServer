using Entitas;
using UnityEngine;

namespace SharedSimulationCode.Systems.InputHandling
{
    /// <summary>
    /// По вводу игрока добавляет силу в направлении ввода
    /// </summary>
    public class MovementSystem : IExecuteSystem
    {
        private readonly GameContext gameContext;
        private readonly IGroup<InputEntity> inputGroup;

        public MovementSystem(Contexts contexts)
        {
            gameContext = contexts.game;
            inputGroup = contexts.input
                .GetGroup(InputMatcher.AllOf(InputMatcher.Movement, InputMatcher.Player));
        }
        
        public void Execute()
        {
            const float maxSpeed = 10f;
            foreach (InputEntity inputEntity in inputGroup)
            {
                Vector2 playerJoystickInput = inputEntity.movement.value;
                ushort playerId = inputEntity.player.id;
                GameEntity playerEntity = gameContext.GetEntityWithPlayer(playerId);
                if (playerEntity == null)
                {
                    Debug.LogError($"Пришло сообщение о движении от игрока, которого нет в комнате. Данные игнорируются. {nameof(playerId)} {playerId}");
                    return;
                }

                if (!playerEntity.hasRigidbody)
                {
                    Debug.LogError("Нет rigidbody");
                    continue;
                }

                // Debug.LogError("Добавление силы");
                Vector3 force = new Vector3(playerJoystickInput.x, 0, playerJoystickInput.y)*10;
                playerEntity.rigidbody.value.AddForce(force, ForceMode.Impulse);
                if(playerEntity.rigidbody.value.velocity.magnitude > maxSpeed)
                {
                    playerEntity.rigidbody.value.velocity = playerEntity.rigidbody.value.velocity.normalized * maxSpeed;
                }
            }
        }
    }
}