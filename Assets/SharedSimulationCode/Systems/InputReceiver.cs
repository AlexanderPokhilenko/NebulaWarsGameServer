using UnityEngine;

namespace SharedSimulationCode.Systems
{
    /// <summary>
    /// Создаёт сущности по вводу игроков.
    /// </summary>
    public class InputReceiver
    {
        private readonly Contexts contexts;
        private readonly InputMessagesMetaHistory messagesMetaHistory;

        public InputReceiver(Contexts contexts, InputMessagesMetaHistory messagesMetaHistory)
        {
            this.contexts = contexts;
            this.messagesMetaHistory = messagesMetaHistory;
        }

        public bool NeedHandle(ushort playerId, int inputId, int tickNumber)
        {
            return messagesMetaHistory.NeedHandle(playerId, inputId);
        }
        
        public void AddMovement(ushort playerId, Vector2 vector2, int tickNumber)
        {
            var inputEntity = GetOrCreateInputEntityForPlayer(playerId);
            inputEntity.ReplaceMovement(vector2);
        }
        
        public void AddAttack(ushort playerId, float attackAngle, int tickNumber)
        {
            var inputEntity = GetOrCreateInputEntityForPlayer(playerId);
            inputEntity.ReplaceAttack(attackAngle);
        }
        
        public void AddExit(ushort playerId)
        {
            var inputEntity = GetOrCreateInputEntityForPlayer(playerId);
            inputEntity.isLeftTheGame = true;
        }
        
        public void AddAbility(ushort playerId, int tickNumber)
        {    
            var inputEntity = GetOrCreateInputEntityForPlayer(playerId);
            inputEntity.isTryingToUseAbility = true;
        }

        private InputEntity GetOrCreateInputEntityForPlayer(ushort playerId)
        {
            InputEntity inputEntity = contexts.input.GetEntityWithPlayer(playerId);
            if (inputEntity == null)
            {
                inputEntity = contexts.input.CreateEntity();
                inputEntity.AddPlayer(playerId);
            }

            return inputEntity;
        }
    }
}