using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Создаёт сущности ввода по вводу игроков.
    /// </summary>
    public class InputReceiver
    {
        private readonly Contexts contexts;

        public InputReceiver(Contexts contexts)
        {
            this.contexts = contexts;
        }
        
        public void AddMovement(ushort playerId, Vector2 vector2)
        {
            var inputEntity = GetOrCreateInputForPlayer(playerId);
            inputEntity.ReplaceMovement(vector2);
        }
        
        public void AddAttack(ushort playerId, float attackAngle)
        {
            var inputEntity = GetOrCreateInputForPlayer(playerId);
            inputEntity.ReplaceAttack(attackAngle);
        }
        
        public void AddExit(ushort playerId)
        {
            var inputEntity = GetOrCreateInputForPlayer(playerId);
            inputEntity.isLeftTheGame = true;
        }
        
        public void AddAbility(ushort playerId)
        {    
            var inputEntity = GetOrCreateInputForPlayer(playerId);
            inputEntity.isTryingToUseAbility = true;
        }

        private InputEntity GetOrCreateInputForPlayer(ushort playerId)
        {
            var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
            if (inputEntity == null)
            {
                inputEntity = contexts.input.CreateEntity();
                inputEntity.AddPlayer(playerId);
            }

            return inputEntity;
        }
    }
}