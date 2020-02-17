﻿using System;
using System.Collections.Concurrent;
using Vector2 = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

namespace Server.GameEngine.StaticMessageSorters
{
    public static class StaticInputMessagesSorter
    {
        public static readonly ConcurrentDictionary<int, Vector2> MovementMessages =
            new ConcurrentDictionary<int, Vector2>();

        public static readonly ConcurrentDictionary<int, float> AttackMessages =
            new ConcurrentDictionary<int, float>();

        public static void Spread()
        {
            ActionForEachMessage(MovementMessages, (inputEntity, joystickPosition) => inputEntity.AddMovement(joystickPosition));
            ActionForEachMessage(AttackMessages, (inputEntity, attackAngle) => inputEntity.AddAttack(attackAngle));

            MovementMessages.Clear();
            AttackMessages.Clear();
        }

        private static void ActionForEachMessage<T>(ConcurrentDictionary<int, T> messages, Action<InputEntity, T> action)
        {
            //TODO: попробовать сделать с помощью Parallel.ForEach
            foreach (var pair in messages)
            {
                //версия C# Unity не умеет деконструировать KeyValuePair
                var playerId = pair.Key;
                var value = pair.Value;

                if (GameEngineMediator.BattlesStorage.PlayersToSessions.TryGetValue(playerId, out var gameSession))
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
                        //Debug.Log("Идентификатор: " + playerId + ", значение: " + value);
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