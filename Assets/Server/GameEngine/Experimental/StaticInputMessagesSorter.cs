using System;
using System.Collections.Concurrent;
using System.Linq;
using log4net;
using Vector2 = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

namespace Server.GameEngine.Experimental
{
    public static class StaticExitMessageSorter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StaticExitMessageSorter));
        //playerId, secretKey
        private static readonly ConcurrentBag<int> ConcurrentBag = new ConcurrentBag<int>();
        
        public static void AddExitMessage(int playerId)
        {
            Log.Error(nameof(AddExitMessage));
            if (!ConcurrentBag.Contains(playerId))
            {
                Log.Error(nameof(AddExitMessage)+" добавление в список");
                ConcurrentBag.Add(playerId);
            }
        }

        public static void Spread()
        {
            while (!ConcurrentBag.IsEmpty)
            {
                ConcurrentBag.TryTake(out int playerId);
                Log.Error($"{nameof(playerId)} {playerId}");
                if (GameEngineMediator.MatchStorageFacade.TryGetMatchByPlayerId(playerId, out var match))
                {
                    Contexts contexts = match.Contexts;

                    if (contexts != null)
                    {
                        var inputEntity = contexts.input.CreateEntity();
                        inputEntity.AddPlayerExit(playerId);
                        Log.Error("Успешное добавление сущности");
                    }
                    else
                    {
                        throw new Exception("Пришло сообщение о выходе из матча от игрока, который не" +
                                            " зарегистрирован");
                    }
                }
                else
                {
                    Log.Error($"Не удалось найти матч для игрока {nameof(playerId)} {playerId}");
                }
            }   
        }
    } 
    
    public static class StaticInputMessagesSorter
    {
        private static readonly ConcurrentDictionary<int, Vector2> MovementMessages =
            new ConcurrentDictionary<int, Vector2>();

        private static readonly ConcurrentDictionary<int, float> AttackMessages =
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

                if (GameEngineMediator.MatchStorageFacade.TryGetMatchByPlayerId(playerId, out var gameSession))
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
    }
}