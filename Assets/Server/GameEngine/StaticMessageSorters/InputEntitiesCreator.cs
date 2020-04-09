using System;
using System.Collections.Concurrent;
using Vector2 = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

namespace Server.GameEngine.Experimental
{
    /// <summary>
    /// Создаёт сущности ввода игрока в контекстах.
    /// </summary>
    public class InputEntitiesCreator
    {
        private readonly MatchStorage matchStorage;
        private readonly ConcurrentDictionary<int, Vector2> movementMessages = new ConcurrentDictionary<int, Vector2>();
        private readonly ConcurrentDictionary<int, float> attackMessages = new ConcurrentDictionary<int, float>();

        public InputEntitiesCreator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public bool TryAddMovementMessage(int playerId, Vector2 vector)
        {
            return movementMessages.TryAdd(playerId, vector);
        }

        public bool TryAddAttackMessage(int playerId, float angle)
        {
            return attackMessages.TryAdd(playerId, angle);
        }
        
        public void Create()
        {
            ActionForEachMessage(movementMessages, (inputEntity, joystickPosition) =>
            {
                inputEntity.AddMovement(joystickPosition);
            });
            ActionForEachMessage(attackMessages, (inputEntity, attackAngle) =>
            {
                inputEntity.AddAttack(attackAngle);
            });

            movementMessages.Clear();
            attackMessages.Clear();
        }

        private void ActionForEachMessage<T>(ConcurrentDictionary<int, T> messages, Action<InputEntity, T> action)
        {
            foreach (var pair in messages)
            {
                var playerId = pair.Key;
                var value = pair.Value;

                if (matchStorage.TryGetMatchByPlayerId(playerId, out Match match))
                {
                    Contexts contexts = match.Contexts;

                    if (contexts != null)
                    {
                        var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
                        if (inputEntity == null)
                        {
                            inputEntity = contexts.input.CreateEntity();
                            inputEntity.AddPlayer(playerId);
                        }

                        action(inputEntity, value);
                    }
                    else
                    {
                        throw new Exception("Пришло сообщение с вводом от игрока, который не зарегистрирован");
                    }
                }
            }
        }
    }
}