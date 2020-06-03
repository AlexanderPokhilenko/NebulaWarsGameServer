using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

namespace Server.GameEngine.Experimental
{
    /// <summary>
    /// Создаёт сущности ввода игрока в контекстах.
    /// </summary>
    public class InputEntitiesCreator
    {
        private readonly MatchStorage matchStorage;
        
        //playerId matchId value
        private readonly ConcurrentDictionary<int, Tuple<int, Vector2>>  movementMessages = new ConcurrentDictionary<int, Tuple<int, Vector2>>();
        //playerId, matchId value
        private readonly ConcurrentDictionary<int, Tuple<int, float>> attackMessages = new ConcurrentDictionary<int, Tuple<int, float>>();
        //playerId, matchId value
        private readonly ConcurrentDictionary<int, Tuple<int,bool>> abilityMessages = new ConcurrentDictionary<int, Tuple<int, bool>>();

        public InputEntitiesCreator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }
        
        public bool TryAddMovementMessage(int matchId, int playerId, Vector2 vector)
        {
            return movementMessages.TryAdd(playerId, new Tuple<int, Vector2>(matchId, vector));
        }

        public bool TryAddAttackMessage(int matchId, int playerId, float angle)
        {
            return attackMessages.TryAdd(playerId, new Tuple<int, float>(matchId, angle));
        }

        public bool TryAddAbilityMessage(int matchId, int playerId, bool ability)
        {
            return abilityMessages.TryAdd(playerId, new Tuple<int, bool>(matchId, ability));
        }

        public void Create()
        {
            ActionForEachMessage(movementMessages, MovementAction);
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

        private void MovementAction(InputEntity inputEntity, Vector2 vector2)
        {
            inputEntity.AddMovement(vector2);
        }

        private void ActionForEachMessage<T>(ConcurrentDictionary<int, Tuple<int, T>> messages, Action<InputEntity, T> action)
        {
            foreach (KeyValuePair<int, Tuple<int, T>> pair in messages)
            {
                int matchId = pair.Value.Item1;
                int playerId = pair.Key;
                T value = pair.Value.Item2;

                if (matchStorage.TryGetMatch(matchId, out Match match))
                {
                    match.AddInputEntity(playerId, action, value);    
                }
            }
        }
    }
}