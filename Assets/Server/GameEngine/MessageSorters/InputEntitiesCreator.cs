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
        private readonly ConcurrentDictionary<int, bool> abilityMessages = new ConcurrentDictionary<int, bool>();

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

        public bool TryAddAbilityMessage(int playerId, bool ability)
        {
            return abilityMessages.TryAdd(playerId, ability);
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
            ActionForEachMessage(abilityMessages, (inputEntity, useAbility) =>
            {
                inputEntity.isTryingToUseAbility = useAbility;
            });
            
            movementMessages.Clear();
            attackMessages.Clear();
            abilityMessages.Clear();
        }

        private void ActionForEachMessage<T>(ConcurrentDictionary<int, T> messages, Action<InputEntity, T> action)
        {
            foreach (var pair in messages)
            {
                var playerId = pair.Key;
                var value = pair.Value;

                if (matchStorage.TryGetMatchByPlayerId(playerId, out Match match))
                {
                    match.AddInputEntity(playerId, action, value);
                }
            }
        }
    }
}