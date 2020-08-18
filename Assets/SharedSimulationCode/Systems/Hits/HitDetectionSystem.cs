using Code.Common;
using Entitas;
using Entitas.Unity;
using Server.GameEngine.Chronometers;
using SharedSimulationCode.LagCompensation;
using SharedSimulationCode.Physics;
using UnityEngine;

namespace SharedSimulationCode.Systems.Hits
{
    /// <summary>
    /// Обнаруживает столкновение пуль
    /// </summary>
    public class HitDetectionSystem : LagCompensationSystem
    {
        private readonly GameContext gameContext;
        private readonly PhysicsRaycaster physicsRaycaster;
        private readonly IGroup<GameEntity> withDamageGroup;
        private readonly ILog log = LogManager.CreateLogger(typeof(HitDetectionSystem));

        public HitDetectionSystem(Contexts contexts, PhysicsRaycaster physicsRaycaster)
        {
            this.physicsRaycaster = physicsRaycaster;
            gameContext = contexts.game;
            //todo уточнить группу (это только пули)
            withDamageGroup = contexts.game
                .GetGroup(GameMatcher.AllOf(GameMatcher.Damage, GameMatcher.Transform));
        }
        
        // public override GameState PastState { get; set; }
        // public override GameState PresentState { get; set; }

        public override void Execute(GameEntity damageEntity)
        {
            if (!damageEntity.hasTransform)
            {
                return;
            }

            if (!damageEntity.hasDamage)
            {
                return;
            }

            Vector3 currentPosition = damageEntity.transform.value.position;
            Vector3 direction = damageEntity.transform.value.rotation * Vector3.forward;
            Vector3 velocity = damageEntity.rigidbody.value.velocity * Chronometer.DeltaTime;

            //Есть столкновение?
            if (!physicsRaycaster.Raycast(currentPosition, direction, velocity.magnitude, 
                out RaycastHit raycastHit))
            {
                return;
            }
            
            EntityLink entityLink = raycastHit.transform.gameObject.GetEntityLink();
            GameEntity warshipEntity = (GameEntity) entityLink.entity;
            if (warshipEntity == null)
            {
                return;
            }
                
            ushort entityId = warshipEntity.id.value;
            //Проверка попадания по самому себе
            if (damageEntity.parentWarship.entity.id.value == entityId)
            {
                log.Error($"Попадание по самому себе parentId = {entityId}");
                return;
            }
            
            log.Debug($"Попадание entityId = {entityId}");
            log.Debug(raycastHit.transform.gameObject.name);
            
            
            GameEntity hitEntity = gameContext.CreateEntity();
            hitEntity.AddHit(damageEntity, warshipEntity);
        }
    }
}