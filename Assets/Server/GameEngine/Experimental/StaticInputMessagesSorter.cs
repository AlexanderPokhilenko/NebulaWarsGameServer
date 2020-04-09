using System;
using System.Collections.Concurrent;
using Vector2 = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

namespace Server.GameEngine.Experimental
{
    public static class StaticInputMessagesSorter
    {
        private static readonly ConcurrentDictionary<int, Vector2> MovementMessages =
            new ConcurrentDictionary<int, Vector2>();

        private static readonly ConcurrentDictionary<int, float> AttackMessages =
            new ConcurrentDictionary<int, float>();

        private static readonly ConcurrentDictionary<int, bool> AbilityMessages =
            new ConcurrentDictionary<int, bool>();

        public static void Spread()
        {
            ActionForEachMessage(MovementMessages, (inputEntity, joystickPosition) => inputEntity.AddMovement(joystickPosition));
            ActionForEachMessage(AttackMessages, (inputEntity, attackAngle) => inputEntity.AddAttack(attackAngle));
            ActionForEachMessage(AbilityMessages, (inputEntity, useAbility) => inputEntity.isTryingToUseAbility = useAbility);

            MovementMessages.Clear();
            AttackMessages.Clear();
            AbilityMessages.Clear();
        }

        private static void ActionForEachMessage<T>(ConcurrentDictionary<int, T> messages, Action<InputEntity, T> action)
        {
            //TODO: попробовать сделать с помощью Parallel.ForEach
            foreach (var pair in messages)
            {
                //версия C# Unity не умеет деконструировать KeyValuePair
                var playerId = pair.Key;
                var value = pair.Value;

                if (GameEngineMediator.MatchStorageFacade.TryGetValueMatchByPlayerId(playerId, out var gameSession))
                {
                    Contexts contexts = gameSession.Contexts;

                    if (contexts != null)
                    {
                        var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
                        if (inputEntity == null)
                        {
                            inputEntity = contexts.input.CreateEntity();
                            inputEntity.AddPlayer(playerId);
                        }

                        action(inputEntity, value);
                        //Log.Info("Идентификатор: " + playerId + ", значение: " + value);
                    }
                    else
                    {
                        throw new Exception("Пришло сообщение с вводом от игрока, который не зарегистрирован");
                    }
                }
            }
        }

        public static bool TryAddMovementMessage(int playerId, Vector2 vector)
        {
            return MovementMessages.TryAdd(playerId, vector);
        }

        public static bool TryAddAttackMessage(int playerId, float angle)
        {
            return AttackMessages.TryAdd(playerId, angle);
        }

        public static bool TryAddAbilityMessage(int playerId, bool ability)
        {
            return AbilityMessages.TryAdd(playerId, ability);
        }
    }
}