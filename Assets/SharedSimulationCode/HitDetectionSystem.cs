using Entitas;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Обнаруживает столкновение пуль
    /// </summary>
    public class HitDetectionSystem : IExecuteSystem
    {
        private readonly PhysicsRaycaster physicsRaycaster;
        private readonly IGroup<GameEntity> withDamageGroup;

        public HitDetectionSystem(Contexts contexts, PhysicsRaycaster physicsRaycaster)
        {
            this.physicsRaycaster = physicsRaycaster;
            //todo уточнить группу (это только пули)
            withDamageGroup = contexts.game
                .GetGroup(GameMatcher.AllOf(GameMatcher.Damage, GameMatcher.Transform));
        }

        public void Execute()
        {
            foreach (var entity in withDamageGroup)
            {
                Vector3 currentPosition = entity.transform.value.position;
                Vector3 previousPosition = Vector3.zero;

                Vector3 direction = currentPosition - previousPosition;
                RaycastHit[] hits = null;
                physicsRaycaster.Raycast(previousPosition, direction, direction.magnitude, hits);

                if (hits != null && hits.Length!=0)
                {
                    Debug.LogError("Столкновение!");
                }
            }
        }
    }
}