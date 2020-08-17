using System.Collections.Concurrent;
using System.Collections.Generic;
using Code.Common;
using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace SharedSimulationCode
{
    /// <summary>
    /// Обнаруживает столкновение пуль
    /// </summary>
    public class HitDetectionSystem : IExecuteSystem
    {
        private readonly GameContext gameContext;
        private readonly PhysicsRaycaster physicsRaycaster;
        private readonly IGroup<GameEntity> withDamageGroup;
        private readonly ILog log = LogManager.CreateLogger(typeof(HitDetectionSystem));
        //костыльный словарь
        private readonly Dictionary<ushort, Vector3> tmpPrevPositions = new Dictionary<ushort, Vector3>();
        
        public HitDetectionSystem(Contexts contexts, PhysicsRaycaster physicsRaycaster)
        {
            this.physicsRaycaster = physicsRaycaster;
            gameContext = contexts.game;
            //todo уточнить группу (это только пули)
            withDamageGroup = contexts.game
                .GetGroup(GameMatcher.AllOf(GameMatcher.Damage, GameMatcher.Transform));
        }

        public void Execute()
        {
            foreach (var damageEntity in withDamageGroup)
            {
                Vector3 currentPosition = damageEntity.transform.value.position;

                //Можно посчитать отдезрк перемещения за кадр?
                if (!tmpPrevPositions.ContainsKey(damageEntity.id.value))
                {
                    tmpPrevPositions.Add(damageEntity.id.value, currentPosition);
                    continue;
                }
                Vector3 previousPosition = tmpPrevPositions[damageEntity.id.value];
                Vector3 delta = currentPosition - previousPosition;

                //Обновить костыльный словарь
                tmpPrevPositions[damageEntity.id.value] = currentPosition;
                
                //Есть столкновение?
                if (!physicsRaycaster.Raycast(previousPosition, delta, delta.magnitude, out RaycastHit raycastHit))
                {
                    continue;
                }
                
                EntityLink entityLink = raycastHit.transform.gameObject.GetEntityLink();
                GameEntity warshipEntity = (GameEntity) entityLink.entity;
                if (warshipEntity == null)
                {
                    continue;
                }
                    
                ushort entityId = warshipEntity.id.value;
                
                
                //Проверка попадания по самому себе
                if (damageEntity.parentWarship.entity.id.value == entityId)
                {
                    log.Error($"Попадание по самому себе parentId = {entityId}");
                    continue;
                }
                
                log.Debug($"Попадание entityId = {entityId}");
                var hitEntity = gameContext.CreateEntity();
                hitEntity.AddHit(damageEntity, warshipEntity);
                    
                log.Debug(raycastHit.transform.gameObject.name);
            }
        }
    }
}